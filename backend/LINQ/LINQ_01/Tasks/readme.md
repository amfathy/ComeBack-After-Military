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
