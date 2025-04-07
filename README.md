FlexFactor Dispute Reconciliation System

Overview:
Importing dispute reports from external payment providers (CSV format)
Comparing against internal dispute records
Detecting and reporting various discrepancies
Generating reconciliation reports in JSON or CSV formats


The system is designed to easily support additional file formats in the future
New report types can be added by implementing the IReport interface
The currency conversion system can be extended to use real-time exchange rates

Technical Details

Framework: .NET8
Design Patterns:

Repository pattern for data access
Strategy pattern for report generation
Dependency Injection for loose coupling
Testing: Comprehensive unit tests with MSTest

Architecture Decisions
SOLID Principles Applied
1.Dependency Inversion Principle: High-level components depend on abstractions
2.Single Responsibility Principle: Each class has a single responsibility (importing, reconciling, reporting)

