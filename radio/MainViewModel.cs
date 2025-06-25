using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace radio
{
    public class MainViewModel
    {
        private readonly DatabaseContext _dbContext;

        public MainViewModel()
        {
            var connectionString = App.Configuration.GetConnectionString("RadioDB");
            _dbContext = new DatabaseContext(connectionString);
        }


        public DataTable GetProductsByCategory(int categoryId)
        {
            string query = @"
                SELECT 
                    t.ID_Товара,
                    t.Название,
                    t.Количество,
                    t.Цена,
                    t.Описание,
                    p.Название AS НазваниеПроизводителя
                FROM Товары t 
                JOIN Производители p ON t.ID_Производителя = p.ID_Производителя
                WHERE t.ID_Категории = @CategoryId";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryId", categoryId)
            };

            try
            {
                return _dbContext.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении товаров: {ex.Message}");
                return new DataTable(); // Возвращаем пустую таблицу при ошибке
            }
        }

        private IEnumerable<Product> ConvertDataTableToProducts(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    Description = row["Description"].ToString(),
                    Manufacturer = row["Manufacturer"].ToString()
                };
            }
        }
    }
}