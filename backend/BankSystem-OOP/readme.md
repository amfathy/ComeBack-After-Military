# Bank System — OOP Practice Task

This is a practice task built to study and apply **C# OOP syntax and concepts** just a hands-on exercise in core object-oriented programming.

## 🎯 Purpose

The goal of this task is to get comfortable with fundamental C# OOP syntax:

- **Abstract classes** (`BankAccount`) and **abstract methods** (`withdraw`)
- **Inheritance** (`SavingAccount` / `CheckingAccount` extending `BankAccount`)
- **Method overriding** (`override withdraw`, `override DisplayInfo`)
- **Interfaces** and **explicit interface implementation** (`ITransferable`)
- **Access modifiers** (`private`, `protected`, `internal`, `readonly`)
- **Encapsulation** (protected `balance` field, controlled via `deposite`/`removeBalance`)
- **Composition** (a `Customer` holding a list of `BankAccount`s)

## 📋 Task Description

Model a simple banking system with the following requirements:

- A `BankAccount` base class holding shared account data (account number, owner name, balance) and shared behavior (`deposite`), while leaving `withdraw` abstract since withdrawal rules differ per account type.
- A `SavingAccount` that can't be overdrawn and earns interest (`calculateInterstRate`).
- A `CheckingAccount` that allows withdrawing beyond the balance up to an `OverdraftLimit`.
- Both account types implement an `ITransferable` interface so money can be transferred from one account to another.
- A `Customer` class that owns multiple accounts and can list them all.
- A `Program.cs` entry point that wires everything together: creates a customer, opens both account types, deposits, withdraws, checks interest, transfers funds, and prints the final account states.

