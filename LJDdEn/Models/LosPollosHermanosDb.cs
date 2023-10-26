using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LJDdEn.Views;
using MySql.Data.MySqlClient;
using static LJDdEn.Models.Ingredient;

namespace LJDdEn.Models
{
    public class LosPollosHermanosDb
    {
        public const string OK = "OK";
        private string connString =
        ConfigurationManager.ConnectionStrings["Lj2Dd1En2Conn"].ConnectionString;
        private string UNKNOWN;

        #region //getmeals
        // GetMeals leest alle rijen uit de databasetabel Meals en voegt deze toe aan een ICollection.
        // Als de ICollection bij aanroep null is, volgt er een ArgumentException.
        // De waarde van GetMeals:
        // - "ok" als er geen fouten waren.
        // - een foutmelding als er wel fouten waren (mogelijk zijn niet alle maaltijden ingelezen).

        public string GetMeals(ICollection<Meal> meals)
        {
            if (meals == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetMeals");
            }

            string methodResult = "unknown";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                SELECT m.mealId, m.name, m.description, m.price
                FROM meals m
            ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Meal meal = new Meal()
                        {
                            MealId = (int)reader["mealId"],
                            Name = (string)reader["name"],
                            Description = reader["description"] == DBNull.Value
                                            ? null
                                            : (string)reader["description"],
                            Price = (decimal)reader["price"],
                        };

                        meals.Add(meal);
                    }

                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetMeals));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }
#endregion
        #region //getingredients
        public string GetIngredients(ICollection<Ingredient> ingredients)
        {
            if (ingredients == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetIngredienten");
            }

            string methodResult = "unknown";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                               SELECT i.ingrediëntId, i.Name, i.Price, i.unitId, u.name as unitName
                               FROM ingredients i
                               INNER JOIN units u ON u.unitId = i.unitId
                               ORDER BY i.Name
   
        ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Ingredient ingredient = new Ingredient()
                        {
                            IngrediëntId = (int)reader["ingrediëntId"],
                            Name = (string)reader["Name"],
                            Price = (decimal)reader["Price"],
                            UnitId = (int)reader["UnitId"],

                            Unit = new Unit()
                            {
                                UnitId = (int)reader["unitId"],
                                Name = (string)reader["unitName"],
                            }
                        };

                        ingredients.Add(ingredient);
                    }

                    methodResult = OK;
                }

                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetIngredients));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }

            return methodResult;
        }
        #endregion

        #region //create

        // CreateIngredient voegt het ingredient object uit de parameter toe aan de database. 
        // Het ingredient object moet aan alle database eisen voldoen. De waarde van CreateIngredient:
        // - "ok" als er geen fouten waren. 
        // - een foutmelding (de melding geeft aan wat er fout was)
        public string CreateIngredient(Ingredient ingredient)
        {

            if (ingredient == null || string.IsNullOrEmpty(ingredient.Name)
                || ingredient.Price < 0 || ingredient.UnitId == 0)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateIngredient");
            }

            string UNKNOWN = null;
            string methodResult = UNKNOWN;

            string connString = null;
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
               INSERT INTO ingredients 
                       (ingrediëntId,  name,  price,  unitId) 
               VALUES  (NULL,         @name, @price, @unitId);
            ";
                    sql.Parameters.AddWithValue("@name", ingredient.Name);
                    sql.Parameters.AddWithValue("@price", ingredient.Price);
                    sql.Parameters.AddWithValue("@unitId", ingredient.UnitId);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        string? OK = null;
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingrediënt {ingredient.Name} kon niet toegevoegd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        
        #endregion
        #region //update
        // UpdateIngredient wijzigt het ingredient met id ingredientId (parameter) met de gegevens uit
        // de parameter ingredient. De gegevens van ingredient moeten aan alle database eisen voldoen.
        // De waarde van UpdateIngredient:
        // - "ok" als er geen fouten waren. 
        // - een foutmelding (de melding geeft aan wat er fout was)
        public string UpdateIngredient(int ingredientId, Ingredient ingredient)
        {
            if (ingredient == null || string.IsNullOrEmpty(ingredient.Name)
                || ingredient.Price < 0 || ingredient.UnitId == 0)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van UpdateIngredient");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"1
                UPDATE ingredients
                SET name = @name, 
                    price = @price,
                    unitId = @unitId
                WHERE ingredientId = @ingredientId;
                ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    sql.Parameters.AddWithValue("@name", ingredient.Name);
                    sql.Parameters.AddWithValue("@price", ingredient.Price);
                    sql.Parameters.AddWithValue("@unitId", ingredient.UnitId);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingrediënt {ingredient.Name} kon niet gewijzigd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(UpdateIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        #endregion
        #region 
        // DeleteIngredient verwijdert het ingredient met de id ingredientId uit de database. De waarde
        // van DeleteIngredient :
        // - "ok" als er geen fouten waren. 
        // - een foutmelding (de melding geeft aan wat er fout was)
        public string DeleteIngredient(int ingredientId)
        {


            string UNKNOWN = null;
            string methodResult = UNKNOWN;

            string connString = null;
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                DELETE 
                FROM ingredients 
                WHERE ingredientId = @ingredientId 
            ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        string? OK = null;
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingredient met id {ingredientId} kon niet verwijderd worden.";
                    }

                }
                catch (Exception e)
                {
                    object DeleteIngredient = null;
                    Console.Error.WriteLine(nameof(DeleteIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        #endregion
        #region //getIngredient
        // GetIngredient leest 1 rij in uit de databasetabel Ingredients. Wordt er een rij gevonden, dan
        // worden de gegevens hiervan in de output parameter ingredient gezet. 
        // Parameters:
        // - ingredientId   : Id van het in te lezen ingredient
        // - ingredient(o)  : null = niet gevonden
        //                    anders nieuw ingredient object met de database gegevens
        // De waarde van GetIngredient:
        // - "ok" als er geen fouten waren. 
        // - een foutmelding, als er wel fouten ware
        public string GetIngredient(int ingredientId, string? OK, out Ingredient? ingredient)
        {
            ingredient = null;
            string UNKNOWN = null;
            string methodResult = UNKNOWN;

            string connString = null;
            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                SELECT i.ingredientId, i.name, i.price, i.unitId, u.name as unitName
                FROM ingredients i
                INNER JOIN units u ON u.unitId = i.unitId
                WHERE i.ingredientId = @ingredientId;
                ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        ingredient = new()
                        {
                            IngrediëntId = (int)reader["ingredientId"],
                            Name = (string)reader["name"],
                            Price = (decimal)reader["price"],
                            UnitId = (int)reader["unitId"],
                            Unit = new Unit()
                            {
                                UnitId = (int)reader["unitId"],
                                Name = (string)reader["unitName"],

                            }
                        };
                    }

                    string NOTFOUND = null;
                    methodResult = ingredient == null ? NOTFOUND : OK;
                }
                catch (Exception e)
                {
                    object GetIngredient = null;
                    Console.Error.WriteLine(nameof(GetIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }
    }
   

    #endregion
}


