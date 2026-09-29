# Design Principles

## 🎓 Credits

This documentation is based on the explanation videos of **Eng. Essam Abdelnaby**:

📺 [Design Principles | مبادئ التصميم — YouTube Playlist](https://www.youtube.com/playlist?list=PL4n1Qos4Tb6ThSyydEJTm7xJ3qEwE8Oyu)

## 🎓 This Repo is
A collection of core object-oriented design principles, each demonstrated with a concrete **before/after** C# refactoring example, UML diagrams, and a full explanation of what problem is being solved and why.

Every folder below tackles one principle in isolation: the "before" code shows the design problem in a small, realistic scenario, and the "after" code shows the refactored solution — with the reasoning spelled out step by step.

---

## 📚 Principles

| Principle | Summary | Docs |
|---|---|---|
| **Encapsulate What Varies** | Identify the part of your code that's likely to change and separate it from the code that's stable — put what changes behind an abstraction. | [📄 Read](./Encapsulate%20What%20Varies/readme.md) |
| **Favor Composition Over Inheritance** | Prefer building objects from smaller, interchangeable parts (HAS-A) over rigid inheritance hierarchies (IS-A), especially when combinations of behavior are needed. | [📄 Read](./Favor-Composition-over-Inheritance/readme.md) |
| **Tightly vs Loosely Coupled** | Depend on abstractions instead of concrete classes so components can be extended, swapped, and tested independently. | [📄 Read](./Tightly-Vs-LooselyCoupled/readme.md) |
| **SOLID — Single Responsibility Principle (SRP)** | A class should have one, and only one, reason to change. | [📄 Read](./SOLID-SRP/readme.md) |
| **SOLID — Open/Closed Principle (OCP)** | Software entities should be open for extension but closed for modification — add new behavior without editing existing, tested code. | [📄 Read](./SOLID-OCP/readme.md) |
| **SOLID — Liskov Substitution Principle (LSP)** | Subtypes must be substitutable for their base types without breaking the correctness of the program. | [📄 Read](./SOLID-LSP/readme.md) |
| **SOLID — Interface Segregation Principle (ISP)** | Clients shouldn't be forced to depend on methods they don't use — split fat interfaces into small, focused ones. | [📄 Read](./SOLID-ISP/readme.md) |
| **SOLID — Dependency Inversion Principle (DIP)** | High-level and low-level modules should both depend on abstractions, not on each other directly. | [📄 Read](./SOLID-DIP/readme.md) |

---

## 🧭 How to Use This Folder

Each principle's folder is self-contained and includes:

- A short statement of the principle
- A **"Before"** code sample showing the design problem
- An **"After"** code sample showing the refactored solution
- A **UML class diagram** for each version
- A comparison table and key takeaway

Start with **Encapsulate What Varies** — it's the foundational idea behind most of the others (OCP, DIP, and Tightly vs Loosely Coupled are really all specific applications of "isolate what changes behind an abstraction").

## 🗂️ Folder Structure

```
Design-Principles/
├── Encapsulate What Varies/
│   └── readme.md
├── Favor-Composition-over-Inheritance/
│   └── readme.md
├── SOLID-DIP/
│   └── readme.md
├── SOLID-ISP/
│   └── readme.md
├── SOLID-LSP/
│   └── readme.md
├── SOLID-OCP/
│   └── readme.md
├── SOLID-SRP/
│   └── readme.md
└── Tightly-Vs-LooselyCoupled/
    └── readme.md
```
