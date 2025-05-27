# Zoo Management System - Lab 1
## Programming Principles Used

### 1. DRY (Don't Repeat Yourself)
We avoid code repetition by encapsulating shared logic in base classes or reusable methods.

- `Animal` is a base class that prevents duplicating `Name`, `Species`, `Age`, and `MakeSound()` signatures.
- Code: [`Program.cs#L10-L24`](./lab-1/Program.cs#L10-L24)

---

### 2. KISS (Keep It Simple, Stupid)
Each class is designed with a single clear purpose. No over-engineering or premature abstraction.

- Example: `Food` class only contains properties relevant to food storage and usage.
- Code: [`Program.cs#L67-L79`](./lab-1/Program.cs#L67-L79)

---

### 3. S — Single Responsibility Principle (SRP)
Each class has only one reason to change:

- `Animal` — animal representation.
- `Enclosure` — enclosure management.
- `ZooWorker` — worker information.
- `ZooInventory` — handles inventory printing.
- Code: [`Program.cs#L93-L134`](./lab-1/Program.cs#L93-L134)

---

### 4. O — Open/Closed Principle (OCP)
The system can be extended without modifying existing code.

- You can add new animal types (e.g., `Reptile`) by extending `Animal` without changing existing classes.

---

### 5. L — Liskov Substitution Principle (LSP)
Subclasses can be used wherever their base class is expected.

- `Mammal` and `Bird` can be passed into `Enclosure.AddAnimal(Animal)` without issues.
- Code: [`Program.cs#L26`](./lab-1/Program.cs#L26) and [`Program.cs#L37`](./lab-1/Program.cs#L37)

---

### 6. I — Interface Segregation Principle (ISP)
While interfaces aren't explicitly used here due to simplicity, the design respects ISP by **not forcing unrelated methods** into classes. Adding interfaces like `ISoundMaking` or `IPrintable` is a logical next step.

- Future-proof structure shown in: [`Program.cs#L23`](./lab-1/Program.cs#L23)

---

### 7. D — Dependency Inversion Principle (DIP)
Although not using interfaces directly, classes depend on abstractions (e.g., `List<Animal>`) rather than low-level concrete implementations.

- Example: `ZooInventory` takes `List<T>` as parameters — easily mockable or replaceable.
- Code: [`Program.cs#L95`](./lab-1/Program.cs#L95)

---

### 8. YAGNI (You Aren’t Gonna Need It)
No extra features or logic that aren’t currently needed. For example, we do not implement feeding logic or file export, as those are not part of the task.

- No unnecessary classes/methods present.

---

### 9. Composition Over Inheritance
Where appropriate, we use **composition** rather than deep inheritance. `Enclosure` **has a list of animals**, rather than inheriting from them.

---

## Conclusion

This project successfully demonstrates the use of core programming principles including **all five SOLID principles**, **DRY**, **KISS**, **YAGNI**, and more, in a compact and readable Zoo management system.
