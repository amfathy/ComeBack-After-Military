# C# Delegates: `delegate`, `Action`, `Func`, `Predicate`

## 1. What is a delegate?

A **delegate** is a type that holds a reference to a method. Think of it as a *type-safe function pointer*: you can store a method in a variable, pass it to another method, and call it later.

```csharp
// 1. Declare the delegate type (defines the method signature)
public delegate int MathOperation(int a, int b);

// 2. Methods that match the signature
static int Add(int a, int b) => a + b;
static int Multiply(int a, int b) => a * b;

// 3. Use it
MathOperation op = Add;
Console.WriteLine(op(2, 3));   // 5

op = Multiply;
Console.WriteLine(op(2, 3));   // 6
```

**Why use delegates?**
- Pass behavior as a parameter (callbacks, strategies)
- Events (`event EventHandler`)
- LINQ (`Where`, `Select`, etc. take delegates)
- Decouple "what to do" from "when to do it"

### Multicast delegates

A delegate can hold multiple methods; they run in order.

```csharp
public delegate void Notify(string message);

static void LogToConsole(string m) => Console.WriteLine($"Console: {m}");
static void LogToFile(string m)    => Console.WriteLine($"File: {m}");

Notify notify = LogToConsole;
notify += LogToFile;      // add
notify("Hello");          // calls both

notify -= LogToConsole;   // remove
notify("Again");          // calls only LogToFile
```

---

## 2. Built-in generic delegates

Declaring a custom delegate every time is tedious, so .NET provides three ready-made families:

| Delegate | Returns | Parameters | Typical use |
|---|---|---|---|
| `Action<...>` | `void` | 0 to 16 | Do something (side effects) |
| `Func<..., TResult>` | a value (**last type** is the return type) | 0 to 16 | Compute/transform something |
| `Predicate<T>` | `bool` | exactly 1 | Test a condition |

---

## 3. `Action`: returns nothing

Use when the method performs work but returns no value.

```csharp
// No parameters
Action sayHi = () => Console.WriteLine("Hi!");
sayHi();

// One parameter
Action<string> greet = name => Console.WriteLine($"Hello, {name}");
greet("Sara");

// Two parameters
Action<string, int> repeat = (text, times) =>
{
    for (int i = 0; i < times; i++)
        Console.WriteLine(text);
};
repeat("Go!", 3);
```

**Real use:** `List<T>.ForEach` takes an `Action<T>`.

```csharp
var names = new List<string> { "Ali", "Mona", "Omar" };
names.ForEach(n => Console.WriteLine(n));
```

---

## 4. `Func`: returns a value

The **last** generic type is always the return type.

```csharp
// Func<TResult>: no params, returns int
Func<int> getFive = () => 5;

// Func<T, TResult>: one param, returns result
Func<int, int> square = x => x * x;
Console.WriteLine(square(4));        // 16

// Func<T1, T2, TResult>
Func<int, int, int> add = (a, b) => a + b;
Console.WriteLine(add(2, 3));        // 5

// Different types
Func<string, int> length = s => s.Length;
Console.WriteLine(length("hello"));  // 5
```

**Real use:** LINQ `Select`, `OrderBy`, `Sum`, etc.

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var squares = numbers.Select(x => x * x);   // Func<int,int>
// 1, 4, 9, 16
```

---

## 5. `Predicate<T>`: returns bool

Equivalent to `Func<T, bool>`, but named for intent: "does this item match?"

```csharp
Predicate<int> isEven = n => n % 2 == 0;
Console.WriteLine(isEven(4));   // True
Console.WriteLine(isEven(7));   // False
```

**Real use:** `List<T>.Find`, `FindAll`, `RemoveAll`, `Exists`.

```csharp
var nums = new List<int> { 1, 2, 3, 4, 5, 6 };

int firstEven       = nums.Find(isEven);       // 2
List<int> allEvens  = nums.FindAll(isEven);    // 2, 4, 6
bool hasEven        = nums.Exists(isEven);     // True
nums.RemoveAll(isEven);                        // nums = 1, 3, 5
```

> **Note:** LINQ methods like `Where` use `Func<T, bool>`, not `Predicate<T>`. The two are *not* interchangeable as types, even though the shape is the same.

---

## 6. Passing delegates as parameters

This is where delegates shine: the caller decides the behavior.

```csharp
static List<int> Filter(List<int> items, Predicate<int> condition)
{
    var result = new List<int>();
    foreach (var item in items)
        if (condition(item))
            result.Add(item);
    return result;
}

static List<int> Transform(List<int> items, Func<int, int> transformer)
    => items.Select(transformer).ToList();

static void Process(List<int> items, Action<int> action)
{
    foreach (var item in items) action(item);
}

var data = new List<int> { 1, 2, 3, 4, 5 };

var big      = Filter(data, x => x > 3);        // 4, 5
var doubled  = Transform(data, x => x * 2);     // 2, 4, 6, 8, 10
Process(data, x => Console.WriteLine(x));       // prints each
```

---

## 7. Named methods vs. lambdas

All three can take either a named method or a lambda.

```csharp
static bool IsPositive(int n) => n > 0;

Predicate<int> p1 = IsPositive;        // method group
Predicate<int> p2 = n => n > 0;        // lambda
Predicate<int> p3 = delegate (int n) { return n > 0; };  // anonymous method (older style)
```

---

## 8. Quick comparison

```csharp
Action<int>        print   = x => Console.WriteLine(x);   // int -> void
Func<int, int>     doubler = x => x * 2;                  // int -> int
Predicate<int>     isBig   = x => x > 100;                // int -> bool
Func<int, bool>    isBig2  = x => x > 100;                // same shape as Predicate
```

| Question | Use |
|---|---|
| Do I need to return something? | No → `Action`, Yes → `Func` |
| Is the return always `bool` with one input? | `Predicate<T>` (or `Func<T,bool>` for LINQ) |
| Need a custom name or `ref`/`out` params? | Declare your own `delegate` |

---

## 9. Full runnable example

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var people = new List<(string Name, int Age)>
        {
            ("Ali", 17), ("Mona", 25), ("Omar", 40), ("Sara", 15)
        };

        // Predicate: who is an adult?
        Predicate<(string Name, int Age)> isAdult = p => p.Age >= 18;
        var adults = people.FindAll(isAdult);

        // Func: build a display string
        Func<(string Name, int Age), string> describe =
            p => $"{p.Name} ({p.Age})";

        // Action: print it
        Action<string> print = Console.WriteLine;

        adults.Select(describe).ToList().ForEach(print);
        // Mona (25)
        // Omar (40)
    }
}
```

---

## 10. Summary

- **Delegate**: a type representing a method signature; holds references to methods.
- **`Action`**: no return value.
- **`Func`**: returns a value (last type parameter).
- **`Predicate<T>`**: takes one `T`, returns `bool`.
- Prefer `Action`/`Func`/`Predicate` over custom delegates unless you need a specific name or `ref`/`out`.

---

# Delegate Practice: Real-World Tasks

## Learning order

Do **not** jump straight to Task 10. Follow this order:

| Task | Focus |
|---|---|
| 1 | Passing a method using your own delegate |
| 2 | A delegate changes an algorithm |
| 3 | A delegate represents a rule |
| 4 | A delegate represents a strategy |
| 5 | A delegate for logging |
| 6 | A delegate for business rules |
| 7 | `Action<T>` |
| 8 | `Func<T, TResult>` |
| 9 | `Predicate<T>` |
| 10 | Combine everything into a backend-style system |

## Important concept

A delegate is a **type that represents a method**.

```csharp
delegate void Logger(string message);
```

This says: *"I can store a method that receives a string and returns nothing."*

```csharp
Logger logger = ConsoleLogger;
logger("Hello");   // executes ConsoleLogger
```

```
METHOD  ->  can be stored in a DELEGATE  ->  can be passed to another method
        ->  different behavior can be injected
```

## Built-in delegates

| Delegate | Meaning | Example |
|---|---|---|
| `Action<T>` | Input -> void | `Action<string>` |
| `Func<T, TResult>` | Input -> Result | `Func<Employee, decimal>` |
| `Predicate<T>` | Input -> bool | `Predicate<Employee>` |

## Mental model

- `List<T>`: "I have a collection."
- `IEnumerable<T>`: "I can iterate through a sequence."
- Delegate: "I have a method/behavior that I can pass around."
- `Action<T>`: "A method that returns void."
- `Func<T, TResult>`: "A method that returns a value."
- `Predicate<T>`: "A method that answers true/false."

---

## Task 1: Order Notification System

**Scenario:** You are building an e-commerce system. When an order is created, the system may need to send an Email, SMS, or Notification.

Create:

```csharp
class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalPrice { get; set; }
}

delegate void NotificationHandler(Order order);
```

Create these methods:

- `SendEmail(Order order)`
- `SendSMS(Order order)`
- `SendNotification(Order order)`

Create `ProcessOrder(Order order, NotificationHandler handler)`. It should process the order and then execute the delegate.

Goal usage:

```csharp
ProcessOrder(order, SendEmail);
ProcessOrder(order, SendSMS);
ProcessOrder(order, SendNotification);
```

**Goal:** Understand that a delegate allows you to pass a method as a parameter.

---

## Task 2: Employee Salary Processing

**Scenario:** A company has different ways to calculate an employee's salary.

Create:

```csharp
class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
}

delegate decimal SalaryCalculator(Employee employee);
```

Create these methods:

- `CalculateRegularSalary(Employee employee)`
- `CalculateSalaryWithBonus(Employee employee)`
- `CalculateSalaryWithOvertime(Employee employee)`

Then create:

```csharp
CalculateFinalSalary(Employee employee, SalaryCalculator calculator)
```

The method should use the supplied calculator.

```csharp
CalculateFinalSalary(employee, CalculateRegularSalary);
CalculateFinalSalary(employee, CalculateSalaryWithBonus);
CalculateFinalSalary(employee, CalculateSalaryWithOvertime);
```

**Goal:** Understand how a delegate lets you change the behavior/algorithm without changing the main method.

---

## Task 3: Employee Filtering

**Scenario:** You have a list of employees and want to retrieve employees based on different business rules.

```csharp
List<Employee> employees;
```

Possible filters:

- Salary greater than 50000
- Department is "IT"
- Experience greater than 5 years
- Employee needs training

Create:

```csharp
delegate bool EmployeeFilter(Employee employee);

List<Employee> FilterEmployees(List<Employee> employees, EmployeeFilter filter)
```

Inside the method, loop through the employees. For every employee call `filter(employee)`; if it returns `true`, add the employee to the result list.

```csharp
FilterEmployees(employees, HighSalaryEmployee);
FilterEmployees(employees, ITEmployee);
FilterEmployees(employees, ExperiencedEmployee);
```

```
Employee -> EmployeeFilter -> true / false
```

**Goal:** Understand a delegate as a **rule**.

---

## Task 4: Payment Processing

**Scenario:** You are building an e-commerce payment system that supports Credit Card, PayPal, and Bank Transfer.

Create:

```csharp
class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
}

delegate bool PaymentProcessor(Payment payment);
```

Create:

- `ProcessCreditCard(Payment payment)`
- `ProcessPayPal(Payment payment)`
- `ProcessBankTransfer(Payment payment)`

Each method returns `true` if the payment succeeds. Then create:

```csharp
ProcessPayment(Payment payment, PaymentProcessor processor)
```

```csharp
ProcessPayment(payment, ProcessCreditCard);
ProcessPayment(payment, ProcessPayPal);
ProcessPayment(payment, ProcessBankTransfer);
```

**Think about:** Why is this better than writing the following?

```csharp
if (paymentMethod == "CreditCard")
{
    ...
}
else if (paymentMethod == "PayPal")
{
    ...
}
else if (paymentMethod == "BankTransfer")
{
    ...
}
```

**Goal:** Understand how delegates can represent different strategies/behaviors.

---

## Task 5: Logging System

**Scenario:** The application needs different logging implementations: Console, File, and Database.

Create:

```csharp
delegate void Logger(string message);
```

- `ConsoleLogger(string message)`
- `FileLogger(string message)`
- `DatabaseLogger(string message)`

Then create `ProcessRequest(Logger logger)`. Inside it, call:

```csharp
logger("Request started");
logger("Loading user");
logger("Request completed");
```

```csharp
ProcessRequest(ConsoleLogger);
ProcessRequest(FileLogger);
ProcessRequest(DatabaseLogger);
```

**Goal:** Understand that the business logic does not need to know *where* the log is stored.

---

## Task 6: E-Commerce Discount System

**Scenario:** Different customers/products can receive different discounts.

```csharp
class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

Discount rules:

| Customer | Discount |
|---|---|
| Regular | 5% |
| VIP | 15% |
| Employee | 30% |
| Black Friday | 40% |

Create:

```csharp
delegate decimal DiscountCalculator(Product product);
```

Separate methods:

- `CalculateRegularDiscount(Product product)`
- `CalculateVIPDiscount(Product product)`
- `CalculateEmployeeDiscount(Product product)`
- `CalculateBlackFridayDiscount(Product product)`

Then create `CalculateFinalPrice(Product product, DiscountCalculator calculator)`.

```csharp
CalculateFinalPrice(product, CalculateRegularDiscount);
CalculateFinalPrice(product, CalculateVIPDiscount);
CalculateFinalPrice(product, CalculateEmployeeDiscount);
CalculateFinalPrice(product, CalculateBlackFridayDiscount);
```

```
Delegate -> Different behavior -> Same main method
```

---

## Task 7: Rewrite Using `Action<T>`

Stop creating your own delegate for the logging system. Instead of:

```csharp
delegate void Logger(string message);
```

use `Action<string>` and rewrite Task 5.

- `ConsoleLogger(string message)`
- `FileLogger(string message)`
- `DatabaseLogger(string message)`

```csharp
ProcessRequest(Action<string> logger)

ProcessRequest(ConsoleLogger);
ProcessRequest(FileLogger);
ProcessRequest(DatabaseLogger);
```

Remember: `Action<T>` means **Input -> T, Return -> void**. So `Action<string>` means `string -> void`.

**Goal:** Understand the built-in `Action` delegate.

---

## Task 8: Rewrite Using `Func<T, TResult>`

Rewrite Task 2 using `Func`. Instead of:

```csharp
delegate decimal SalaryCalculator(Employee employee);
```

use `Func<Employee, decimal>`:

```csharp
CalculateFinalSalary(Employee employee, Func<Employee, decimal> calculator)
```

Use `CalculateRegularSalary`, `CalculateSalaryWithBonus`, `CalculateSalaryWithOvertime`.

```csharp
CalculateFinalSalary(employee, CalculateRegularSalary);
CalculateFinalSalary(employee, CalculateSalaryWithBonus);
CalculateFinalSalary(employee, CalculateSalaryWithOvertime);
```

Remember: `Func<TInput, TResult>`, so `Func<Employee, decimal>` means `Employee -> decimal`.

**Goal:** Understand the built-in `Func` delegate.

---

## Task 9: Employee Search Using `Predicate<T>`

A `Predicate<T>` represents `T -> bool`, so `Predicate<Employee>` means `Employee -> true/false`.

Create:

```csharp
List<Employee> FindEmployees(List<Employee> employees, Predicate<Employee> condition)
```

The method returns employees for which `condition(employee)` returns `true`.

Create these conditions:

- `HighSalaryEmployee`
- `ITEmployee`
- `ExperiencedEmployee`
- `EmployeeNeedsTraining`

```csharp
FindEmployees(employees, HighSalaryEmployee);
FindEmployees(employees, ITEmployee);
FindEmployees(employees, ExperiencedEmployee);
```

**Goal:** Understand Predicate as a condition/rule.

---

## Task 10: Real Backend-Style Order Processing

Combine everything into one realistic **Order Processing System**.

```csharp
class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
}
```

The system supports four different behaviors:

### 1. Order validation: `Predicate<Order>`

Possible rules:

- Order ID must be greater than 0
- Customer name cannot be empty
- Total price must be greater than 0
- Order status must be valid

```csharp
Predicate<Order> validation;
```

### 2. Payment processing: `Func<Order, bool>`

Possible methods: Credit Card, PayPal, Bank Transfer. Each returns `true` (payment succeeded) or `false` (payment failed).

### 3. Logging: `Action<string>`

Possible loggers: Console, File, Database.

```csharp
logger("Order processing started");
```

### 4. Notification: `Action<Order>`

Possible notifications: Email, SMS, Push Notification.

### Putting it together

```csharp
ProcessOrder(
    Order order,
    Predicate<Order> validation,
    Func<Order, bool> payment,
    Action<string> logger,
    Action<Order> notification
)
```

The method should:

1. Validate the order
2. Log the beginning of processing
3. Process the payment
4. Update the order status
5. Log the result
6. Send notification

Final usage should look like:

```csharp
ProcessOrder(
    order,
    ValidateOrder,
    ProcessCreditCard,
    ConsoleLogger,
    SendEmail
);
```
