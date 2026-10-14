# LINQ Session 9: Joining (`Join` and `GroupJoin`)

Joins **combine two sequences** by matching a key in one with a key in the other. They are the LINQ version of SQL's `JOIN`.

| Operator | SQL equivalent | Result |
|---|---|---|
| `Join` | `INNER JOIN` | One row **per matching pair** |
| `GroupJoin` | `LEFT JOIN` + grouping | One row **per outer item**, with a **collection** of its matches |
| `GroupJoin` + `DefaultIfEmpty` + `SelectMany` | `LEFT OUTER JOIN` | Every outer item, even without a match |

We use two related lists:

```
Departments                    Employees
 Id | Name                      Id | FullName       | DepartmentId
  1 | IT                         3 | Wallace Buck   |   1  ───┐
  2 | HR                         5 | Jacqueline ... |   1  ───┤  matches Department Id 1
  3 | Finance                    ...                         
```

The **key** that links them: `Employee.DepartmentId` = `Department.Id`.

---

## 1. `Join` (inner join)

Returns a result only when **both** sides have a match.

### Query syntax

```csharp
var query = from emp in employees
            join dept in departments on emp.DepartmentId equals dept.Id
            select new { emp.FullName, dept.Name };
```

### Method syntax

```csharp
var query = employees.Join(
    departments,                    // the inner sequence
    emp  => emp.DepartmentId,       // key from the outer (employees)
    dept => dept.Id,                // key from the inner (departments)
    (emp, dept) => new { emp.FullName, dept.Name });   // result for each match
```

| Parameter | Meaning |
|---|---|
| 1st | The sequence to join with (inner) |
| 2nd | Key selector for the **outer** (the one you call `.Join` on) |
| 3rd | Key selector for the **inner** |
| 4th | How to build one result from a matching pair |

### Key points
- **Unmatched items disappear** from the result: an employee with an invalid `DepartmentId`, or a department with no employees, is not in the output.
- The result keeps the **order of the outer sequence**.
- One outer item matching several inner items produces **several rows**.
- In query syntax the keys are written `outerKey equals innerKey`. The word is `equals` (not `==`) and **the order matters**: the outer variable must be on the left, otherwise it will not compile.
- `Join` is **fast**: it builds a hash table of the inner keys. It is much better than nesting a `Where` inside a loop.

---

## 2. `GroupJoin`

Instead of one row per **pair**, you get one row per **outer item** plus a **collection** of everything that matched it.

### Method syntax

```csharp
var query = departments.GroupJoin(
    employees,
    dept => dept.Id,
    emp  => emp.DepartmentId,
    (dept, emps) => new
    {
        Department = dept.Name,
        Employees  = emps          // IEnumerable<Employee> (can be empty)
    });

foreach (var group in query)
{
    Console.WriteLine(group.Department);
    foreach (var emp in group.Employees)
        Console.WriteLine($"   {emp.FullName}");
}
```

### Query syntax: `join ... into`

```csharp
var query = from dept in departments
            join emp in employees on dept.Id equals emp.DepartmentId into empGroup
            select new { Department = dept.Name, Employees = empGroup };
```

> `join` **without** `into` = `Join`.
> `join` **with** `into` = `GroupJoin`.

### Key points
- **Every outer item appears**, even if nothing matches. Its collection is simply empty.
- The result is a hierarchy: department → its employees. No repeated department name per row.
- It is perfect for "each X with its list of Y" and for "count per X, including zero".

### Careful with aggregates on empty groups

```csharp
(dept, emps) => new { dept.Name, Avg = emps.Average(e => e.Salary) }
// throws InvalidOperationException for a department with no employees!

(dept, emps) => new { dept.Name, Avg = emps.Any() ? emps.Average(e => e.Salary) : 0 }
// safe
```

`Count()`, `Sum()` work fine on an empty group (they return 0). `Average`, `Max`, `Min` on non-nullable numbers do **not**.

---

## 3. Left outer join

Sometimes you want **all** items from the left side, with `null`/default for missing matches. Use `GroupJoin` + `DefaultIfEmpty`.

### Query syntax

```csharp
var query = from emp in employees
            join dept in departments on emp.DepartmentId equals dept.Id into deptGroup
            from d in deptGroup.DefaultIfEmpty()
            select new { emp.FullName, Department = d?.Name ?? "No Department" };
```

### Method syntax

```csharp
var query = employees
    .GroupJoin(departments, e => e.DepartmentId, d => d.Id,
               (e, ds) => new { e, ds })
    .SelectMany(x => x.ds.DefaultIfEmpty(),
                (x, d) => new { x.e.FullName, Department = d?.Name ?? "No Department" });
```

How it works:
1. `GroupJoin` pairs each employee with the (possibly empty) list of matching departments.
2. `DefaultIfEmpty()` turns an empty list into a list with **one `null`**.
3. `SelectMany` flattens it back into rows, so the employee still appears.

### Join type summary

| You want | Use |
|---|---|
| Only items that match on both sides | `Join` |
| Every item of the left side, matched or not | `GroupJoin` + `DefaultIfEmpty` (or just `GroupJoin` for grouped output) |
| Every item of the right side | Swap the two sequences and do a left join |
| Every item of both sides (full outer) | Left join `Union` right join |

---

## 4. Joining more than two sequences

Chain joins in query syntax:

```csharp
var query = from emp in employees
            join dept in departments on emp.DepartmentId equals dept.Id
            join loc  in locations   on dept.LocationId  equals loc.Id
            select new { emp.FullName, dept.Name, loc.City };
```

### Joining on more than one key (composite key)

Use anonymous types on both sides. The **property names must match**:

```csharp
join b in bonuses
    on new { e.DepartmentId, e.HireYear }
    equals new { b.DepartmentId, HireYear = b.Year }
```

---

## 5. `Join` vs `Where` + loops vs `GroupBy`

| Goal | Best tool |
|---|---|
| Combine rows from two lists | `Join` |
| Each parent with its children, even with none | `GroupJoin` |
| Group items of **one** list by a property | `GroupBy` (Session 8) |
| Group **after** an inner join | `Join` then `GroupBy` (departments with no employees disappear) |

`GroupJoin` vs `Join + GroupBy`:

```csharp
// GroupJoin: Legal appears with an empty list
departments.GroupJoin(employees, d => d.Id, e => e.DepartmentId, (d, es) => new { d.Name, Count = es.Count() });

// Join + GroupBy: Legal is missing (no matching pair was ever created)
employees.Join(departments, e => e.DepartmentId, d => d.Id, (e, d) => d.Name)
         .GroupBy(name => name);
```

---

## Cheat sheet

| I want... | Use |
|---|---|
| Employee with department name | `Join` |
| Each department with its employees | `GroupJoin` |
| Count per department including 0 | `GroupJoin` + `Count()` |
| All employees, even without department | `GroupJoin` + `DefaultIfEmpty` + `SelectMany` |
| Departments without employees | `GroupJoin` then `Where(x => !x.Employees.Any())` |
| Join three lists | Two `join` clauses |
| Query syntax: inner / group join | `join ... on a equals b` / `join ... into g` |

## Run the examples

```
RunJoin()                    → inner join
RunJoinQuerySyntax()         → inner join
RunGroupJoin()               → group join
RunGroupJoinQuerySyntax()    → group join
```

## Things to notice in the sample code

- **The method names are swapped.** `RunJoin` is written in **query** syntax and `RunJoinQuerySyntax` is written in **method** syntax. The same happens with `RunGroupJoin` (query syntax) and `RunGroupJoinQuerySyntax` (method syntax). Rename them or point it out to students.
- **`RunGroupJoin` selects only `empGroup`**, so the department name is lost, and the output only shows separators and employee names. `RunGroupJoinQuerySyntax` keeps the department name in an anonymous object, which is the more useful pattern.
- Only one example is called in `Main` (the others are commented out). Remove the `//` to run them all.
- An empty department prints an empty group. That shows the key difference from `Join`.
