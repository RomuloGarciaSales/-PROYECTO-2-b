using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class Vehiculo
{
    public string Marca { get; set; }
    public string Color { get; set; }
    public string Placa { get; set; }
    public string Tipo { get; set; }
    public int HoraEntrada { get; set; }
    public string CodigoEstacionamientoAsignado { get; set; }

    public Vehiculo(string marca, string color, string placa, string tipo, int horaEntrada)
    {
        Marca = marca;
        Color = color;
        Placa = placa;
        Tipo = tipo;
        HoraEntrada = horaEntrada;
        CodigoEstacionamientoAsignado = "";
    }

    public override string ToString()
    {
        return $"Placa: {Placa}, Marca: {Marca}, Color: {Color}, Tipo: {Tipo}, Hora Entrada: {HoraEntrada}h, Estacionado en: {CodigoEstacionamientoAsignado}";
    }
}

public class Estacionamiento
{
    public string Placa { get; set; }
    public string TipoDesignado { get; set; }
    public Vehiculo VehiculoOcupante { get; set; }
    public bool Ocupado => VehiculoOcupante != null;
    public Estacionamiento(string codigo, string tipoDesignado)
    {
        Placa = codigo;
        TipoDesignado = tipoDesignado;
        VehiculoOcupante = null;
    }
}

class Program
{
    static Estacionamiento[,] mapaEstacionamientos;
    static int cantidadPisosConfigurada;
    static int estacionamientosPorPisoConfigurado;
    static Random random = new Random();
    static string[] marcasPosibles = { "Honda", "Mazda", "Hyundai", "Toyota", "Suzuki" };
    static string[] coloresPosibles = { "Rojo", "Azul", "Negro", "Gris", "Blanco" };
    static string[] tiposVehiculoPosibles = { "Moto", "Sedan", "SUV" };

    static void Main(string[] args)
    {
        ConfigurarSistema();
        if (mapaEstacionamientos != null)
        {
            MenuPrincipal();
        }
        else
        {
            Console.WriteLine("Error en la configuración inicial. El programa terminará.");
            Console.WriteLine("Presione cualquier tecla para salir.");
            Console.ReadKey();
        }
    }
    //Datos del usuario para configurar el parqueo
    static void ConfigurarSistema()
    {
        Console.WriteLine("--- Configuración para el Estacionamiento ---");
        estacionamientosPorPisoConfigurado = LeerEnteroPositivo("Ingrese la cantidad de estacionamientos por piso: ");
        cantidadPisosConfigurada = LeerEnteroPositivo("Ingrese la cantidad de pisos habilitados: ");
        int cantidadMotos = LeerEnteroNoNegativo("Ingrese la cantidad de estacionamientos tipo 'Moto': ");
        int cantidadSUVs = LeerEnteroNoNegativo("Ingrese la cantidad de estacionamientos tipo 'SUV': ");
        int totalEstacionamientosCalculado = cantidadPisosConfigurada * estacionamientosPorPisoConfigurado;
        //Validación espacios
        if (cantidadMotos + cantidadSUVs > totalEstacionamientosCalculado)
        {
            Console.WriteLine("Error: La suma de estacionamientos para Motos y SUVs excede el total de espacios disponibles.");
            mapaEstacionamientos = null; 
            return;
        }
        //Crear un nuevo estacionamiento
        mapaEstacionamientos = new Estacionamiento[cantidadPisosConfigurada, estacionamientosPorPisoConfigurado];
        int motosAsignadas = 0;
        int suvsAsignadas = 0;
        //Tipo de estacionamiento
        for (int i = 0; i < cantidadPisosConfigurada; i++)
        {
            char letraPiso = (char)('A' + i);
            for (int p = 0; p < estacionamientosPorPisoConfigurado; p++)
            {
                string codigoEstacionamiento = $"{letraPiso}{p + 1}";
                string tipoDesignadoParaEsteEspacio;
                if (motosAsignadas < cantidadMotos)
                {
                    tipoDesignadoParaEsteEspacio = "Moto";
                    motosAsignadas++;
                }
                else if (suvsAsignadas < cantidadSUVs)
                {
                    tipoDesignadoParaEsteEspacio = "SUV";
                    suvsAsignadas++;
                }
                else
                {
                    tipoDesignadoParaEsteEspacio = "Sedan";
                }
                mapaEstacionamientos[i, p] = new Estacionamiento(codigoEstacionamiento, tipoDesignadoParaEsteEspacio);
            }
        }
        Console.WriteLine("Sistema configurado exitosamente.\n");
    }
    //MAPA DEL ESTACIONAMIENTO
    static void MostrarMapaEstacionamientos(string tipoVehiculoParaMostrarDisponibilidad = null)
    {
        Console.WriteLine("\n--- Mapa de Estacionamientos ---");
        if (tipoVehiculoParaMostrarDisponibilidad != null)
        {
            Console.WriteLine($"(Mostrando disponibilidad para: {tipoVehiculoParaMostrarDisponibilidad})");
        }
        Console.WriteLine("X: Ocupado o tipo no compatible / [Codigo]: Disponible y compatible");

        for (int i = 0; i < cantidadPisosConfigurada; i++)
        {
            Console.WriteLine(); 
            for (int j = 0; j < estacionamientosPorPisoConfigurado; j++)
            {
                Estacionamiento est = mapaEstacionamientos[i, j];
                string estacionamientoDisplay;
                int anchoContenido = 4;
                if (est.Ocupado)
                {
                    estacionamientoDisplay = "X";
                }
                else
                {
                    if (tipoVehiculoParaMostrarDisponibilidad != null && est.TipoDesignado != tipoVehiculoParaMostrarDisponibilidad)
                    {
                        estacionamientoDisplay = "X";
                    }
                    else
                    {
                        estacionamientoDisplay = est.Placa;
                    }
                }
                Console.Write($"[{estacionamientoDisplay.PadRight(anchoContenido)}] ");
            }
        }
        Console.WriteLine("\n---------------------------------");
    }
    //MENÚ PRINCIPAL DENTRO DEL PARQUEO
    static void MenuPrincipal()
    {
        string opcion;
        do
        {
            Console.WriteLine("--- Menú Principal del Centro Comercial ---");
            Console.WriteLine("1. Ingresar un vehículo manualmente");
            Console.WriteLine("2. Ingresar lote de vehículos");
            Console.WriteLine("3. Encontrar un vehículo por placa");
            Console.WriteLine("4. Retirar un vehículo y calcular pago");
            Console.WriteLine("5. Salir del sistema");
            Console.Write("Seleccione una opción: ");
            opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    IngresarVehiculoManualmente();
                    break;
                case "2":
                    IngresarLoteVehiculos();
                    break;
                case "3":
                    EncontrarVehiculo();
                    break;
                case "4":
                    RetirarVehiculo();
                    break;
                case "5":
                    Console.WriteLine("Gracias por usar el sistema. ¡Hasta pronto!");
                    break;
                default:
                    Console.WriteLine("Por favor, ingrese una opción válida.");
                    break;
            }
            if (opcion != "5")
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        } while (opcion != "5");
    }
    //GENERAR PLACA ALEATORIA Y EN LISTA
    static string GenerarPlacaAleatoria()
    {
        StringBuilder placa = new StringBuilder();
        for (int i = 0; i < 3; i++)
        {
            placa.Append((char)random.Next('A', 'Z' + 1));
        }
        for (int i = 0; i < 3; i++)
        {
            placa.Append(random.Next(0, 10));
        }
        return placa.ToString();
    }
    //VERIFICAR EXISTENCIA
    static bool PlacaYaExiste(string placa)
    {
        for (int i = 0; i < cantidadPisosConfigurada; i++)
        {
            for (int j = 0; j < estacionamientosPorPisoConfigurado; j++)
            {
                if (mapaEstacionamientos[i, j].Ocupado && mapaEstacionamientos[i, j].VehiculoOcupante.Placa == placa)
                {
                    return true;
                }
            }
        }
        return false;
    }
    //INGRESAR DATOS DEL VEHÍCULO
    static void IngresarVehiculoManualmente()
    {
        Console.WriteLine("--- Ingresar Vehículo Manualmente ---");
        Console.Write("Marca del vehículo: ");
        string marca = Console.ReadLine();
        Console.Write("Color del vehículo: ");
        string color = Console.ReadLine();
        //Placa válida
        string placa;
        while (true)
        {
            Console.Write("Placa del vehículo (6 caracteres alfanuméricos, se convertirá a mayúsculas): ");
            placa = Console.ReadLine().ToUpper();
            if (placa.Length == 6 && placa.All(char.IsLetterOrDigit))
            {
                if (PlacaYaExiste(placa))
                {
                    Console.WriteLine("Ya existe un vehículo con esta placa en el estacionamiento.");
                }
                else
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine("Placa inválida. Debe tener 6 caracteres alfanuméricos.");
            }
        }
        //Vehículo Válido
        string tipoVehiculo;
        while (true)
        {
            Console.Write("Tipo del vehículo (Moto, Sedan, SUV): ");
            tipoVehiculo = Console.ReadLine();
            if (tipoVehiculo == "Moto" || tipoVehiculo == "Sedan" || tipoVehiculo == "SUV")
                break;
            Console.WriteLine("Tipo inválido. Debe ser Moto, Sedan o SUV.");
        }
        //INGRESAR HORA
        int horaEntrada;
        while (true)
        {
            Console.Write("Hora de entrada (número entero entre 6 y 20): ");
            if (int.TryParse(Console.ReadLine(), out horaEntrada) && horaEntrada >= 6 && horaEntrada <= 20) 
            {
                break;
            }
            Console.WriteLine("Hora inválida. Debe ser un número entre 6 y 20.");
        }
        //INGRESAR UN VEHÍCULO NUEVO
        Vehiculo nuevoVehiculo = new Vehiculo(marca, color, placa, tipoVehiculo, horaEntrada);
        Console.WriteLine($"Mostrando estacionamientos disponibles para tipo '{tipoVehiculo}':");
        MostrarMapaEstacionamientos(tipoVehiculo);
        string codigoEstacionamientoSeleccionado;
        bool estacionamientoAsignado = false;
        while (!estacionamientoAsignado)
        {
            Console.Write($"Ingrese el código del estacionamiento a ocupar (ej. A1) para {tipoVehiculo}: ");
            codigoEstacionamientoSeleccionado = Console.ReadLine().ToUpper(); // Convertir a mayúsculas para consistencia
            bool codigoEncontradoEnMapa = false;
            for (int i = 0; i < cantidadPisosConfigurada; i++)//Buscar las columnas (pisos) seleccionados
            {
                for (int j = 0; j < estacionamientosPorPisoConfigurado; j++) //Buscar las filas (estacionamientos) seleccionados
                {
                    if (mapaEstacionamientos[i, j].Placa == codigoEstacionamientoSeleccionado)
                    {
                        codigoEncontradoEnMapa = true;
                        if (mapaEstacionamientos[i, j].Ocupado)
                        {
                            Console.WriteLine("El estacionamiento seleccionado está ocupado.");
                        }
                        else if (mapaEstacionamientos[i, j].TipoDesignado != tipoVehiculo)
                        {
                            Console.WriteLine($"Error: El estacionamiento {codigoEstacionamientoSeleccionado} es para tipo '{mapaEstacionamientos[i, j].TipoDesignado}', no para '{tipoVehiculo}'.");
                        }
                        else
                        {
                            mapaEstacionamientos[i, j].VehiculoOcupante = nuevoVehiculo;
                            nuevoVehiculo.CodigoEstacionamientoAsignado = mapaEstacionamientos[i, j].Placa;
                            Console.WriteLine($"Vehículo con placa {placa} ingresado exitosamente en {codigoEstacionamientoSeleccionado}.");
                            estacionamientoAsignado = true;
                        }
                        goto FinBusquedaCodigoManual;
                    }
                }
            }
            FinBusquedaCodigoManual:;
            if (!codigoEncontradoEnMapa && !estacionamientoAsignado)
            {
                Console.WriteLine("Error: Código de estacionamiento no válido o no encontrado.");
            }
        }
    }
    //Ingresar estacionamientos ocupados aleatoriamente
    static void IngresarLoteVehiculos()
    {
        Console.WriteLine("\n--- Ingresar Lote de Vehículos Automáticamente ---");
        int numVehiculosAIngresar = random.Next(2, 7);
        Console.WriteLine($"Se intentará ingresar {numVehiculosAIngresar} vehículos de forma aleatoria.");
        List<string> vehiculosIngresadosInfo = new List<string>();
        int vehiculosRealmenteIngresados = 0;

        for (int i = 0; i < numVehiculosAIngresar; i++)
        {
            string marca = marcasPosibles[random.Next(marcasPosibles.Length)];
            string color = coloresPosibles[random.Next(coloresPosibles.Length)];

            string placa;
            int intentosPlaca = 0;
            const int MAX_INTENTOS_PLACA = 100;
            bool placaValidaObtenida = false;

            do
            {
                placa = GenerarPlacaAleatoria();
                if (!PlacaYaExiste(placa))
                {
                    placaValidaObtenida = true;
                    break;
                }
                intentosPlaca++;
            } while (intentosPlaca < MAX_INTENTOS_PLACA);

            if (!placaValidaObtenida)
            {
                Console.WriteLine($"Advertencia: No se pudo generar una placa única para el vehículo aleatorio #{i + 1} después de {MAX_INTENTOS_PLACA} intentos. Saltando este vehículo.");
                continue;
            }
            string tipo = tiposVehiculoPosibles[random.Next(tiposVehiculoPosibles.Length)];
            int horaEntrada = random.Next(6, 21);
            Vehiculo vehiculoAleatorio = new Vehiculo(marca, color, placa, tipo, horaEntrada);
            bool estacionadoExitosamente = false;
            for (int v_piso = 0; v_piso < cantidadPisosConfigurada; v_piso++)
            {
                for (int j_spot = 0; j_spot < estacionamientosPorPisoConfigurado; j_spot++)
                {
                    if (!mapaEstacionamientos[v_piso, j_spot].Ocupado && mapaEstacionamientos[v_piso, j_spot].TipoDesignado == tipo)
                    {
                        mapaEstacionamientos[v_piso, j_spot].VehiculoOcupante = vehiculoAleatorio;
                        vehiculoAleatorio.CodigoEstacionamientoAsignado = mapaEstacionamientos[v_piso, j_spot].Placa;
                        vehiculosIngresadosInfo.Add($"Placa: {placa}, Tipo: {tipo}, Estacionamiento: {mapaEstacionamientos[v_piso, j_spot].Placa}");
                        vehiculosRealmenteIngresados++;
                        estacionadoExitosamente = true;
                        break;
                    }
                }
                if (estacionadoExitosamente)
                {
                    break;
                }
            }
        }

        if (vehiculosRealmenteIngresados > 0)
        {
            Console.WriteLine($"\nSe ingresaron exitosamente {vehiculosRealmenteIngresados} de {numVehiculosAIngresar} vehículos intentados en el lote:");
            foreach (string info in vehiculosIngresadosInfo)
            {
                Console.WriteLine("- " + info);
            }
        }
        else
        {
            Console.WriteLine($"No se pudo ingresar ningún vehículo del lote (de {numVehiculosAIngresar} intentos). Verifique disponibilidad de espacios compatibles.");
        }
        MostrarMapaEstacionamientos();
    }
    //BUSCAR VEHÍCULO MANUALMENTE
    static void EncontrarVehiculo()
    {
        Console.WriteLine("--- Encontrar Vehículo por Placa ---");
        Console.Write("Ingrese el número de placa a buscar: ");
        string placaBuscada = Console.ReadLine().ToUpper();

        if (string.IsNullOrWhiteSpace(placaBuscada) || placaBuscada.Length != 6)
        {
            Console.WriteLine("Placa inválida. Debe tener 6 caracteres alfanuméricos.");
            return;
        }
        for (int i = 0; i < cantidadPisosConfigurada; i++)
        {
            for (int j = 0; j < estacionamientosPorPisoConfigurado; j++)
            {
                Estacionamiento est = mapaEstacionamientos[i, j];
                if (est.Ocupado && est.VehiculoOcupante.Placa == placaBuscada)
                {
                    Console.WriteLine("Vehículo encontrado.");
                    Console.WriteLine(est.VehiculoOcupante.ToString());
                    return;
                }
            }
        }
        Console.WriteLine($"No se encontró ningún vehículo con la placa {placaBuscada} en el estacionamiento.");
    }
    //TARIFA ESTACIONAMIENTO O CALCULAR EL MONTO
    static int CalcularTarifa(int tiempoEstadiaHoras)
    {
        if (tiempoEstadiaHoras <= 1) return 0;  // 0 – 1 hora: cortesía
        if (tiempoEstadiaHoras <= 4) return 15;  // 2 – 4 horas: Q15
        if (tiempoEstadiaHoras <= 7) return 45;  // 5 – 7 horas: Q45
        if (tiempoEstadiaHoras <= 12) return 60; // 8 – 12 horas: Q60
        return 150; // Más de 12 horas: Q150
    }
    static void RetirarVehiculo()
    {
        Console.WriteLine("--- Retirar Vehículo y Procesar Pago ---");
        Console.Write("Ingrese el código del estacionamiento del vehículo a retirar (ej. A1):");
        string codigoBuscado = Console.ReadLine().ToUpper();
        Estacionamiento estacionamientoARetirar = null;
        int filaRetirar = -1, colRetirar = -1;
        for (int i = 0; i < cantidadPisosConfigurada; i++)
        {
            for (int j = 0; j < estacionamientosPorPisoConfigurado; j++)
            {
                if (mapaEstacionamientos[i, j].Placa == codigoBuscado)
                {
                    estacionamientoARetirar = mapaEstacionamientos[i, j];
                    filaRetirar = i;
                    colRetirar = j;
                    goto EstacionamientoEncontradoParaRetiro;
                }
            }
        }
//RETIRAR VEHÍCULO
    EstacionamientoEncontradoParaRetiro:;
        if (estacionamientoARetirar == null)
        {
            Console.WriteLine($"El código de estacionamiento '{codigoBuscado}' no existe.");
            return;
        }
        if (!estacionamientoARetirar.Ocupado)
        {
            Console.WriteLine($"El estacionamiento '{codigoBuscado}' ya está vacío.");
            return;
        }
        Vehiculo vehiculoARetirar = estacionamientoARetirar.VehiculoOcupante;
        int horaEntradaVehiculo = vehiculoARetirar.HoraEntrada;
        int maxEstadiaPosibleHoy = 22 - horaEntradaVehiculo;
        if (maxEstadiaPosibleHoy < 0) maxEstadiaPosibleHoy = 0;
        int horaSalidaSimulada = random.Next(horaEntradaVehiculo, 23); 
        if (horaSalidaSimulada < horaEntradaVehiculo) horaSalidaSimulada = horaEntradaVehiculo;
        int tiempoEstadiaHoras = horaSalidaSimulada - horaEntradaVehiculo;
        int maxHorasEstadiaMismoDia = 22 - horaEntradaVehiculo;
        if (maxHorasEstadiaMismoDia < 0) maxHorasEstadiaMismoDia = 0; 
        tiempoEstadiaHoras = random.Next(0, maxHorasEstadiaMismoDia + 1); 
        Console.WriteLine($"Datos del Vehículo a Retirar: {vehiculoARetirar.ToString()}");
        Console.WriteLine($"Hora de Entrada registrada: {horaEntradaVehiculo}h");
        Console.WriteLine($"Tiempo de estadía (generado aleatoriamente para simulación): {tiempoEstadiaHoras} horas.");
        //Calcular Tarifa
        int montoAPagar = CalcularTarifa(tiempoEstadiaHoras);
        Console.WriteLine($"Monto total a pagar: Q{montoAPagar}");
        if (montoAPagar == 0 && tiempoEstadiaHoras <=1)
        {
            Console.WriteLine("Estadía de cortesía. No se requiere pago.");
        }
        else
        {
            Console.WriteLine("Seleccione el método de pago:");
            Console.WriteLine("1. Tarjeta de crédito/débito");
            Console.WriteLine("2. Efectivo");
            Console.WriteLine("3. Sticker recargable");
            Console.Write("Opción de pago: ");
            string metodoPago = Console.ReadLine();
            //MÉTODO DE PAGO
            switch (metodoPago)
            {
                case "1":
                    Console.WriteLine($"Pago de Q{montoAPagar} procesado con tarjeta. ¡Gracias!");
                    break;
                case "3":
                    Console.WriteLine($"Pago de Q{montoAPagar} procesado con sticker recargable. ¡Gracias!");
                    break;
                case "2":
                    int montoRecibido;
                    while (true)
                    {
                        Console.Write($"Ingrese el monto en efectivo para pagar Q{montoAPagar}: Q");
                        if (int.TryParse(Console.ReadLine(), out montoRecibido))
                        {
                            if (montoRecibido >= montoAPagar)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Monto insuficiente. Por favor, ingrese una cantidad igual o mayor al monto a pagar.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Entrada inválida. Por favor, ingrese un número.");
                        }
                    }
                    int vuelto = montoRecibido - montoAPagar;
                    Console.WriteLine($"Monto recibido: Q{montoRecibido}.");
                    if (vuelto > 0)
                    {
                        Console.WriteLine($"Su vuelto es de: Q{vuelto}");
                        Console.WriteLine("Vuelto correspondiente:");
                        int[] billetes = { 100, 50, 20, 10, 5};
                        int vueltoRestante = vuelto;
                        foreach (int bill in billetes)
                        {
                            if (vueltoRestante >= bill)
                            {
                                int cantidadBilletes = vueltoRestante / bill;
                                Console.WriteLine($" - {cantidadBilletes} {(bill > 1 ? "billete(s)" : "moneda(s)")} de Q{bill}");
                                vueltoRestante %= bill;
                            }
                        }
                    } else {
                         Console.WriteLine("Pago exacto recibido. No hay vuelto.");
                    }
                    Console.WriteLine("Pago en efectivo completado.");
                    break;
                default:
                    Console.WriteLine("Método de pago no reconocido. Se asumirá el pago como completado para fines de esta simulación.");
                    break;
            }
        }
        mapaEstacionamientos[filaRetirar, colRetirar].VehiculoOcupante = null;
        Console.WriteLine($"\nVehículo con placa {vehiculoARetirar.Placa} ha sido retirado del estacionamiento {codigoBuscado}.");
        Console.WriteLine("El espacio ahora está disponible.");
        MostrarMapaEstacionamientos();
    }
    static int LeerEnteroPositivo(string mensaje)
    {
        int valor;
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine(), out valor) && valor > 0)
            {
                return valor;
            }
            Console.WriteLine("Valor inválido. Debe ser un número entero positivo (mayor que 0).");
        }
    }
    static int LeerEnteroNoNegativo(string mensaje)
    {
        int valor;
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine(), out valor) && valor >= 0)
            {
                return valor;
            }
            Console.WriteLine("Valor inválido. Debe ser un número entero no negativo (0 o mayor).");
        }
    }
}