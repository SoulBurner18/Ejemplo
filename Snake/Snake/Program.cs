using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Snake
{
    class Program
    {
        enum Direccion
        {
            Arriba,
            Abajo,
            Izq,
            Der
        }

        class Punto
        {
            public Punto(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public Punto()
            {

            }
            public int X { get; set; }
            public int Y { get; set; }
        }

        static List<Punto> snake = new List<Punto>() { new Punto(10, 10) };
        static bool vivo = true;
        static Direccion direccion = Direccion.Der;
        static int time = 0;
        static bool canMove = true;
        static Punto posComida = new Punto();

        static void Main(string[] args)
        {

            Console.Title = "Viborita pedorra";
            Console.SetWindowSize(25, 20);
            Thread threadTeclas = new Thread(Teclas);
            threadTeclas.SetApartmentState(ApartmentState.STA);
            threadTeclas.Start();

            SpawnComida();

            while (vivo)
            {
                Mover();

                Thread.Sleep(time);
            }

            threadTeclas.Abort();

        }

        private static void SpawnComida()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int x, y;
            x = rand.Next(1, Console.WindowWidth - 1);
            y = rand.Next(1, Console.WindowHeight - 1);

            while (snake.Where(n => n.X == x && n.Y == y).Any())
            {
                x = rand.Next(0, Console.WindowWidth - 1);
                y = rand.Next(0, Console.WindowHeight - 1);
            }
            posComida = new Punto(x, y);
            Console.SetCursorPosition(x, y);
            Console.Write("*");
        }

        private static void Teclas()
        {
            while (vivo)
            {
                if (canMove)
                {
                    if (direccion != Direccion.Abajo && Keyboard.IsKeyDown(Key.Up))
                    {
                        direccion = Direccion.Arriba;
                    }
                    else if (direccion != Direccion.Arriba && Keyboard.IsKeyDown(Key.Down))
                    {
                        direccion = Direccion.Abajo;
                    }
                    else if (direccion != Direccion.Der && Keyboard.IsKeyDown(Key.Left))
                    {
                        direccion = Direccion.Izq;
                    }
                    else if (direccion != Direccion.Izq && Keyboard.IsKeyDown(Key.Right))
                    {
                        direccion = Direccion.Der;
                    }
                    else if (Keyboard.IsKeyDown(Key.Escape))
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }

        private static void Mover()
        {
            Punto posAux = null;
            canMove = false;
            for (int i = 0; i < snake.Count; i++)
            {
                Console.SetCursorPosition(snake[i].X, snake[i].Y);
                Console.Write(" ");

                if (i == 0)
                {
                    posAux = new Punto(snake[0].X, snake[0].Y);
                    if (direccion == Direccion.Arriba)
                    {
                        snake[i].Y -= 1;
                        time = 200;
                    }
                    else if (direccion == Direccion.Abajo)
                    {
                        snake[i].Y += 1;
                        time = 200;
                    }
                    else if (direccion == Direccion.Izq)
                    {
                        snake[i].X -= 1;
                        time = 100;
                    }
                    else if (direccion == Direccion.Der)
                    {
                        snake[i].X += 1;
                        time = 100;
                    }
                }
                else
                {
                    var posAux2 = new Punto(posAux.X, posAux.Y);
                    posAux = new Punto(snake[i].X, snake[i].Y);
                    snake[i] = new Punto(posAux2.X, posAux2.Y);
                }
                Console.SetCursorPosition(snake[i].X, snake[i].Y);
                Console.Write("O");

            }
            canMove = true;
            DetectarColision();
        }

        private static void DetectarColision()
        {
            var cabeza = snake.First();
            if (cabeza.X == posComida.X && cabeza.Y == posComida.Y)
            {
                if (direccion == Direccion.Arriba)
                {
                    snake.Add(new Punto(cabeza.X, cabeza.Y + 1));
                }
                else if (direccion == Direccion.Abajo)
                {
                    snake.Add(new Punto(cabeza.X, cabeza.Y - 1));
                }
                else if (direccion == Direccion.Der)
                {
                    snake.Add(new Punto(cabeza.X - 1, cabeza.Y));
                }
                else if (direccion == Direccion.Izq)
                {
                    snake.Add(new Punto(cabeza.X + 1, cabeza.Y));
                }
                posComida = null;
                SpawnComida();
            }
            /*else
            {
                var Ulti = snake.Last();
                var anteUlt = snake[snake.Count - 2];
                //abajo
                if (Ulti.X == anteUlt.X && Ulti.Y + 1 == anteUlt.Y)
                {
                    snake.Add(new Punto(cabeza.X, cabeza.Y - 1));
                }//arriba
                else if (Ulti.X == anteUlt.X && Ulti.Y - 1 == anteUlt.Y)
                {
                    snake.Add(new Punto(cabeza.X, cabeza.Y + 1));
                }//Derecha
                else if (Ulti.X + 1 == anteUlt.X && Ulti.Y == anteUlt.Y)
                {
                    snake.Add(new Punto(cabeza.X - 1, cabeza.Y));
                }
                else if (Ulti.X - 1 == anteUlt.X && Ulti.Y == anteUlt.Y)
                {
                    snake.Add(new Punto(cabeza.X + 1, cabeza.Y));
                }

            }*/

            if (snake.Where(n => n.X == snake[0].X && n.Y == snake[0].Y && !n.Equals(snake[0])).Any())
            {
                Perder();
            }
            if (snake[0].X <= 0 || snake[0].X >= Console.WindowWidth-1 || snake[0].Y <= 0 || snake[0].Y >= Console.WindowHeight -1)
            {
                Perder();
            }

        }

        private static void Perder()
        {
            vivo = false;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Perdiste we");
            Console.ReadLine();
        }

    }
}

