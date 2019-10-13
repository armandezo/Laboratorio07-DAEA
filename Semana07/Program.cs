using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semana07
{
    public class Program
    {
        public static DataClases2DataContext context = new DataClases2DataContext();


        public static void Main(string[] args)
        {
            Console.WriteLine("----------------------Consultas normales-----------------------------");

            //IntroToLinq();
            //DataSource();
            //filtering();
            //ordering();
            //grouping();
            //grouping2();
            //joining();

            Console.WriteLine("----------------------------Funciones Lambda--------------------------------");
            //IntroToLinqLambda();
            //DataSourceLambda();
            //filteringLambda();
            //orderingLambda();
            //groupingLambda();
            //grouping2Lambda();
            //joiningLambda();

            //PedidoUltimos5();
            //PrecioPorCantidad200();
            MasdeDosProductos();
            Console.ReadKey();
        }

        //consultas

        static void IntroToLinq()
        {
            int[] numbers = new int[7] { 1, 2, 3, 4, 5, 6, 7 };
            var numQuery = from n in numbers where (n % 2) == 0 select n;

            foreach(int num in numQuery)
            {
                Console.WriteLine(num);
            }
        }

        static void DataSource()
        {
            var queryAllCustomer = from c in context.clientes select c;

            foreach (var cliente  in queryAllCustomer)
            {
                Console.WriteLine(cliente.NombreCompañia);
            }
        }
        static void filtering()
        {
            var queryAllCustomer = from c in context.clientes where c.Ciudad == "Londres" select c;

            foreach (var cliente in queryAllCustomer)
            {
                Console.WriteLine(cliente.Ciudad);
            }
           
        }

        static void ordering()
        {
            var queryAllCustomer = from c in context.clientes
                                   where c.Ciudad == "Londres"
                                   orderby c.NombreCompañia ascending
                                   select c;

            foreach (var cliente in queryAllCustomer)
            {
                Console.WriteLine(cliente.NombreCompañia);
            }
            
        }

        static void grouping()
        {
            var queryCustomerByCity = from c in context.clientes
                                      group c by c.Ciudad;

            foreach (var customerGroup in queryCustomerByCity)
            {
                Console.WriteLine(customerGroup.Key);
                foreach (var customer in customerGroup)
                {
                    Console.WriteLine(customer.NombreCompañia);
                }
            }
        }

        static void grouping2()
        {
            var queryCustomerByCity = from c in context.clientes
                                      group c by c.Ciudad into custGroup
                                      where custGroup.Count() > 2
                                      orderby custGroup.Key
                                      select custGroup;

            foreach (var item in queryCustomerByCity)
            {
                Console.WriteLine(item.Key);
            }
        }

        static void joining()
        {
            var innerJoinQuery = from c in context.clientes
                                 join dist in context.Pedidos on c.idCliente equals dist.IdCliente
                                 select new
                                 {
                                     customerName = c.NombreCompañia,
                                     DistribucionName = dist.PaisDestinatario
                                 };

            foreach (var item in innerJoinQuery)
            {
                Console.WriteLine(item.customerName);
            }
        }

        //consultas lambda

        static void IntroToLinqLambda()
        {
            int[] numbers = new int[7] { 1, 2, 3, 4, 5, 6, 7 };
            var numQuery = numbers.Where(x => (x % 2) == 0);

            foreach (int num in numQuery)
            {
                Console.WriteLine(num);
            }
        }

        static void DataSourceLambda()
        {
            var queryAllCustomer = context.clientes;

            foreach (var cliente in queryAllCustomer)
            {
                Console.WriteLine(cliente.NombreCompañia);
            }
        }
        static void filteringLambda()
        {
            var queryAllCustomer = context.clientes.Where(x => x.Ciudad == "Londres");

            foreach (var cliente in queryAllCustomer)
            {
                Console.WriteLine(cliente.Ciudad);
            }
        }

        static void orderingLambda()
        {
            var queryAllCustomer = context.clientes.Where(x => x.Ciudad == "Londres").OrderBy(x => x.NombreCompañia);

            foreach (var cliente in queryAllCustomer)
            {
                Console.WriteLine(cliente.NombreCompañia);
            }
        }

        static void groupingLambda()
        {
            var queryCustomerByCity = context.clientes.GroupBy(c => c.Ciudad);

            foreach (var customerGroup in queryCustomerByCity)
            {
                Console.WriteLine(customerGroup.Key);
                foreach (var customer in customerGroup)
                {
                    Console.WriteLine(customer.NombreCompañia);
                }
            }
            
        }

        static void grouping2Lambda()
        {
            var queryCustomerByCity = context.clientes.GroupBy(x => x.Ciudad).Where(x => x.Count() > 2).OrderBy(x => x.Key);

            foreach (var item in queryCustomerByCity)
            {
                Console.WriteLine(item.Key);
            }
        }

        static void joiningLambda()
        {
            var innerJoinQuery = context.clientes.Join(
                context.Pedidos,
                cli => cli.idCliente,
                dist => dist.IdCliente,
                (cli, dist) => new { customerName = cli.NombreCompañia, DistribucionName = dist.PaisDestinatario });

            foreach (var item in innerJoinQuery)
            {
                Console.WriteLine(item.customerName);
            }
        }
        //Ejercicios

        static void PedidoUltimos5()
        {
            var pedidos = context.Pedidos;
            var ultimoPedido = pedidos.Max(x => x.FechaPedido);
            var lista = pedidos.Where(x => x.FechaPedido > ultimoPedido.Value.AddYears(-5));

            foreach (var item in lista)
            {
                Console.WriteLine(item.FechaPedido);
            }
        }

        static void ClientesConDosPedidos()
        {
            var lista = context.clientes.Where(x => x.Pedidos.Count > 2);

            foreach (var item in lista)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }
        static void PrecioPorCantidad200()
        {
            var lista = (from ped in context.Pedidos
                         join dped in context.detallesdepedidos on ped.IdPedido equals dped.idpedido
                         group dped by dped.idpedido into grupo
                         select new {id = grupo.Key, suma = grupo.Sum(x => x.cantidad * x.preciounidad) }).Where(x => x.suma > 200).ToList();

            foreach (var item in lista)
            {
               Console.Write($"Id {item.id} ");
                Console.WriteLine($"suma {item.suma}");
            }
        }
        static void MasdeDosProductos()
        {
            var provedores = context.proveedores.Where(x => x.productos.Count() > 2).ToList();

            foreach (var item in provedores)
            {
                Console.WriteLine(item.nombrecontacto);
            }
        }

        static void PedidosEnMasDe3()
        {
            var productos = context.detallesdepedidos.Join(
                context.productos,
                dped => dped.idproducto,
                prod => prod.idproducto,
                (dped, prod) => new { dped, prod }).GroupBy(x => x.dped.idpedido);

            foreach (var item in productos)
            {
                Console.WriteLine(item.nombrecontacto);
            }
        }



    }
}
