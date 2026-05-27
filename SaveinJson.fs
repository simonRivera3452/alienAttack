module Game.Save 

open System
open System.IO
open System.Text.Json
open System.Text.Json.Serialization
open Game.types
open Game.States

// 1. Configuramos las opciones EXACTAMENTE como lo hizo tu profesor
let options = 
    JsonFSharpOptions.Default()
        .ToJsonSerializerOptions()

// 2. Mantenemos la ruta absoluta para que el archivo no se "pierda" al cerrar el juego
let rutaBase = AppDomain.CurrentDomain.BaseDirectory
let nombreArchivo = Path.Combine(rutaBase, "record.json")

// Guarda el estado completo en un archivo JSON
let guardarPartida (state: State) =
    try
        // Pasamos el estado y las opciones directamente como segundo argumento
        let json = JsonSerializer.Serialize(state, options)
        File.WriteAllText(nombreArchivo, json)
    with 
    | ex -> printfn "Error al guardar: %s" ex.Message

// Carga el estado completo desde el archivo JSON
let cargarPartida () =
    if File.Exists nombreArchivo then
        try
            let json = File.ReadAllText(nombreArchivo)
            let estadoCargado = JsonSerializer.Deserialize<State>(json, options)
            
            // Forzamos a que inicie en el juego
            { estadoCargado with ProgramState = Running; RedibujarPantalla = true }
        with
        | ex -> 
            // 🚨 DIAGNÓSTICO 1: Si el JSON se rompe al leerlo, aquí veremos por qué
            printfn "\n[DEBUG SAVE] El archivo existe, pero falló al cargarse!"
            printfn "[DEBUG SAVE] Error real: %s" ex.Message
            printfn "Presiona cualquier tecla para iniciar con estado inicial..."
            Console.ReadKey() |> ignore
            estadoInicial
    else
        // 🚨 DIAGNÓSTICO 2: Si el archivo ni siquiera se creó
        printfn "\n[DEBUG SAVE] Error: El archivo '%s' no existe." nombreArchivo
        printfn "Presiona cualquier tecla para iniciar..."
        Console.ReadKey() |> ignore
        estadoInicial