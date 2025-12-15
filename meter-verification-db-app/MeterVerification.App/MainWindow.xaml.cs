using MeterVerification.Core.Models;
using MeterVerification.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MeterVerification.App
{
    public partial class MainWindow : Window
    {
        private AppDbContext _context;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                var connectionString = "Host=localhost;Port=5432;Database=meter_verification_db;Username=postgres;Password=root";

                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(connectionString)
                    .Options;

                _context = new AppDbContext(options);

                if (_context.Database.CanConnect())
                {
                    tbStatus.Text = "База данных подключена";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                tbStatus.Text = "Ошибка подключения";
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var verifications = _context.Verifications
                    .Include(v => v.Organization)
                    .Include(v => v.DeviceType)
                    .Include(v => v.DeviceModification)
                    .ToList();

                dataGrid.ItemsSource = verifications;

                tbCount.Text = $"Записей: {verifications.Count}";
                tbStatus.Text = $"Данные загружены ({verifications.Count} записей)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                tbStatus.Text = "Ошибка загрузки";
            }
        }

        private void BtnAddTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();

                var org1 = new Organization { Name = "ООО 'МИЦ'" };
                var org2 = new Organization { Name = "АО 'СВЕТЛАНА'" };
                _context.Organizations.AddRange(org1, org2);
                _context.SaveChanges();

                var type1 = new DeviceType
                {
                    RegistrationNumber = "74458-19",
                    TypeName = "Газоанализаторы",
                    Designation = "М 03"
                };
                var type2 = new DeviceType
                {
                    RegistrationNumber = "15200-06",
                    TypeName = "Термопреобразователи",
                    Designation = "ТСМУ-205"
                };
                _context.DeviceTypes.AddRange(type1, type2);
                _context.SaveChanges();

                var mod1 = new DeviceModification
                {
                    Name = "исп. М03-01",
                    DeviceTypeId = type1.Id
                };
                var mod2 = new DeviceModification
                {
                    Name = "ТСМУ-205",
                    DeviceTypeId = type2.Id
                };
                _context.DeviceModifications.AddRange(mod1, mod2);
                _context.SaveChanges();

                var verifications = new[]
                {
                    new Verification
                    {
                        VersionId = "test-1",
                        OrganizationId = org1.Id,
                        DeviceTypeId = type1.Id,
                        DeviceModificationId = mod1.Id,
                        SerialNumber = "7076",
                        VerificationDate = DateTime.Now.AddMonths(-1),
                        ValidUntil = DateTime.Now.AddMonths(11),
                        CertificateNumber = "С-001/2025",
                        IsSuitable = true
                    },
                    new Verification
                    {
                        VersionId = "test-2",
                        OrganizationId = org2.Id,
                        DeviceTypeId = type2.Id,
                        DeviceModificationId = mod2.Id,
                        SerialNumber = "9121",
                        VerificationDate = DateTime.Now.AddMonths(-2),
                        ValidUntil = DateTime.Now.AddMonths(10),
                        CertificateNumber = "С-002/2025",
                        IsSuitable = true
                    },
                    new Verification
                    {
                        VersionId = "test-3",
                        OrganizationId = org1.Id,
                        DeviceTypeId = type1.Id,
                        DeviceModificationId = mod1.Id,
                        SerialNumber = "15194",
                        VerificationDate = DateTime.Now.AddMonths(-3),
                        ValidUntil = DateTime.Now.AddMonths(-1), // Просрочен!
                        CertificateNumber = "С-003/2024",
                        IsSuitable = false
                    }
                };

                _context.Verifications.AddRange(verifications);
                _context.SaveChanges();

                MessageBox.Show($"Добавлено тестовых данных:\n" +
                               $"2 организации\n" +
                               $"2 типа СИ\n" +
                               $"2 модификации\n" +
                               $"3 поверки", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                tbStatus.Text = "Тестовые данные добавлены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                tbStatus.Text = "Ошибка добавления";
            }
        }
    }
}