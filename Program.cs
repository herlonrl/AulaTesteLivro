namespace AulaTesteLivro
{

    public delegate bool Filter<T>(T element);

    public interface IFilter<T>
    {
        bool Match(T element);
    }

    public class Livro
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int AnoPublicacao { get; set; }
        public decimal Preco { get; set; }
    }

    public class LivroRepository
    {
        private List<Livro> livros;

        public LivroRepository(List<Livro> livros)
        {
            this.livros = livros;
        }

        public List<Livro> Filtrar(IFilter<Livro> filtro)
        {
            return livros.Where(x => filtro.Match(x)).ToList();
        }
    }

    public class LivroFilter
    {
        public class FiltroAnoPublicacao : IFilter<Livro>
        {
            private int anoMinimo;

            public FiltroAnoPublicacao(int anoMinimo)
            {
                this.anoMinimo = anoMinimo;
            }

            public bool Match(Livro livro)
            {
                return livro.AnoPublicacao >= anoMinimo;
            }
        }

        public class FiltroPrecoMaximo : IFilter<Livro>
        {
            private decimal precoMaximo;

            public FiltroPrecoMaximo(decimal precoMaximo)
            {
                this.precoMaximo = precoMaximo;
            }

            public bool Match(Livro livro)
            {
                return livro.Preco <= precoMaximo;
            }
        }

        public class FiltroTituloContemPalavra : IFilter<Livro>
        {
            private string palavraChave;

            public FiltroTituloContemPalavra(string palavraChave)
            {
                this.palavraChave = palavraChave;
            }

            public bool Match(Livro livro)
            {
                return livro.Titulo.Contains(palavraChave, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            // Criando alguns livros para testar
            List<Livro> livros = new List<Livro>
        {
            new Livro { Titulo = "Aprendendo C#", Autor = "Autor1", AnoPublicacao = 2020, Preco = 50.0m },
            new Livro { Titulo = "C# Avançado", Autor = "Autor2", AnoPublicacao = 2022, Preco = 70.0m },
            new Livro { Titulo = "Design Patterns", Autor = "Autor3", AnoPublicacao = 2019, Preco = 60.0m }
        };

            // Criando instância do LivroRepository
            LivroRepository repository = new LivroRepository(livros);

            // Testando os filtros
            Console.WriteLine("Livros publicados a partir de 2020:");
            ImprimirLivros(repository.Filtrar(new LivroFilter.FiltroAnoPublicacao(2020)));

            Console.WriteLine("\nLivros até o preço de 60.0:");
            ImprimirLivros(repository.Filtrar(new LivroFilter.FiltroPrecoMaximo(60.0m)));

            Console.WriteLine("\nLivros com a palavra 'C#' no título:");
            ImprimirLivros(repository.Filtrar(new LivroFilter.FiltroTituloContemPalavra("C#")));
        }

        static void ImprimirLivros(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                Console.WriteLine($"Título: {livro.Titulo}, Autor: {livro.Autor}, Ano: {livro.AnoPublicacao}, Preço: {livro.Preco:C}");
            }
        }
    }

}
