using NUnit.Framework;
using PayrollManagementSystem.Models;
using PayrollManagementSystem.Repository;
using PayrollManagementSystem.Repository.Interfaces;
using PayrollManagementSystem.Services;
namespace PayrollManagementSystem.Tests
{
    [TestFixture]
    public class Tests
    {
        private IEmployeeService _employeeService;
        private IEmployeeRepository _employeeRepository;
        private ITaxServices _taxServices;
        private ITaxRepository _taxRepository;
        private IPayrollTRepository _payrollTRepository;
        private IpayrollServices _payrollServices;

        [SetUp]
        public void Setup()
        {
            _employeeRepository = new EmployeeRepository();
            _employeeService = new EmployeeService(_employeeRepository);
            _taxRepository = new TaxRepository();
            _taxServices = new TaxServices(_taxRepository);
            _payrollTRepository = new PayrollRepository();
                _payrollServices = new PayrollServices(_payrollTRepository);
        }

        [Test]
        public void AddEmployee_ShouldAddEmployeeSuccessfully()
        {
            var employee = new Employee
            {
                FirstName = "Anita",
                LastName = "Das",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "F",
                Email = "anita.das@example.com",
                PhoneNumber = "0987654324",
                Address = "4545 Elm Srt",
                Position = "Developer",
                JoiningDate = DateTime.Now,
                TerminationDate = null
            };
            int result = _employeeService.AddEmployee(employee);
            Assert.That(result, Is.EqualTo(1));
        }
        [Test]
        public void CalculateNetSalaryAfterDeductions_ShouldReturnCorrectNetSalary()
        {
            var employeeId = 1;
            var taxYear = DateTime.Now.Year;
            decimal taxableIncome = 10800;
            decimal taxAmount = 1080;
            decimal netSalary = _taxServices.Netsalary(employeeId, taxYear);
            decimal expectedNetSalary = taxableIncome - taxAmount;
            Assert.That(netSalary, Is.EqualTo(expectedNetSalary).Within(0.01));


        }
        [Test]
        public void ProcessprayrollmultiplEmployees()
        {
            var payrolls = new List<Payroll>
            {
                new Payroll {  EmployeeID = 1, BasicSalary = 50000, OvertimePay = 10000, Deductions = 7000, PayPeriodStartDate = new DateTime(2023, 1, 1), PayPeriodEndDate = new DateTime(2023, 1, 31) },
                new Payroll {  EmployeeID = 2, BasicSalary = 70000, OvertimePay = 15000, Deductions = 10000, PayPeriodStartDate = new DateTime(2023, 1, 1), PayPeriodEndDate = new DateTime(2023, 1, 31) }
            };

            foreach (var payroll in payrolls)
            {
                var generatedPayroll = _payrollServices.GeneratePayroll(
           payroll.EmployeeID,
           payroll.PayPeriodStartDate,
           payroll.PayPeriodEndDate,
           payroll.BasicSalary,
           payroll.OvertimePay,
           payroll.Deductions
       );
            }
            var payrollListForEmployee1 = _payrollServices.GetPayrollsForEmployee(1);
            Assert.That(payrollListForEmployee1, Is.Not.Null);
            Assert.That(payrollListForEmployee1.Count, Is.GreaterThan(0));

            var payrollListForEmployee2 = _payrollServices.GetPayrollsForEmployee(2);
            Assert.That(payrollListForEmployee2, Is.Not.Null);
            Assert.That(payrollListForEmployee2.Count, Is.GreaterThan(0));
        }
        [Test]
        public void ProcessGrosssalary()
        {
            var payrolls = new List<Payroll>
            { 
                new Payroll {  EmployeeID = 9, BasicSalary = 70000, OvertimePay = 15000, Deductions = 10000, PayPeriodStartDate = new DateTime(2023, 1, 1), PayPeriodEndDate = new DateTime(2023, 1, 31) }
            };
            foreach (var payroll in payrolls)
            {
                var generatedPayroll = _payrollServices.GeneratePayroll(
           payroll.EmployeeID,
           payroll.PayPeriodStartDate,
           payroll.PayPeriodEndDate,
           payroll.BasicSalary,
           payroll.OvertimePay,
           payroll.Deductions);
            }
            var payrollListForEmployee1 = _payrollServices.GetPayrollsForEmployee(9);
            Assert.That(payrollListForEmployee1, Is.Not.Null);
            Assert.That(payrollListForEmployee1.Count, Is.GreaterThan(0));
            int empid = 9;
            decimal result = _payrollServices.Getgrosssalary(empid);
            Assert.That(result, Is.EqualTo(85000.00).Within(0.01));
        }


    }

}
