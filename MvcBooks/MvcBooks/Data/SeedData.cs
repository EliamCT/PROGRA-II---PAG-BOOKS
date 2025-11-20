using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcBooks.Models;

namespace MvcBooks.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new BooksContext(
                serviceProvider.GetRequiredService<DbContextOptions<BooksContext>>());

            if (context.Books.Any())
            {
                return;
            }

            context.Books.AddRange(
                new Book { Title = "Cien años de soledad", Author = "Gabriel García Márquez", Genre = "Realismo mágico", Year = 1967, Publisher = "Sudamericana", ISBN = "9780307474728" },
                new Book { Title = "El Señor de los Anillos", Author = "J.R.R. Tolkien", Genre = "Fantasía épica", Year = 1954, Publisher = "Allen & Unwin", ISBN = "9780618640157" },
                new Book { Title = "1984", Author = "George Orwell", Genre = "Distopía", Year = 1949, Publisher = "Secker & Warburg", ISBN = "9780451524935" },
                new Book { Title = "Harry Potter y la piedra filosofal", Author = "J.K. Rowling", Genre = "Fantasía", Year = 1997, Publisher = "Bloomsbury", ISBN = "9788478884452" },
                new Book { Title = "Don Quijote de la Mancha", Author = "Miguel de Cervantes", Genre = "Novela", Year = 1605, Publisher = "Francisco de Robles", ISBN = "9788491050297" },
                new Book { Title = "El principito", Author = "Antoine de Saint-Exupéry", Genre = "Fábula", Year = 1943, Publisher = "Reynal & Hitchcock", ISBN = "9780156012195" },
                new Book { Title = "Crimen y castigo", Author = "Fiódor Dostoievski", Genre = "Novela psicológica", Year = 1866, Publisher = "The Russian Messenger", ISBN = "9780140449136" },
                new Book { Title = "Orgullo y prejuicio", Author = "Jane Austen", Genre = "Romance", Year = 1813, Publisher = "T. Egerton", ISBN = "9780141439518" },
                new Book { Title = "El código Da Vinci", Author = "Dan Brown", Genre = "Misterio", Year = 2003, Publisher = "Doubleday", ISBN = "9780307474278" },
                new Book { Title = "Sapiens: De animales a dioses", Author = "Yuval Noah Harari", Genre = "Ensayo", Year = 2011, Publisher = "Harvill Secker", ISBN = "9780062316097" }
            );

            context.SaveChanges();
        }
    }
}