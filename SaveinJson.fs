module Game.Save 
open Game.Engine
open System.IO
open System.Text.Json
open Game.types
open Game.States
let nombreArchivo = "record.json"

// Guarda el estado completo en un archivo JSON
let guardarPartida (state: State) =
    try
        let json = JsonSerializer.Serialize(state)
        File.WriteAllText(nombreArchivo, json)
    with 
    | ex -> printfn "Error al guardar: %s" ex.Message
let cargarPartida () =
    if File.Exists nombreArchivo then
        try
            let json = File.ReadAllText(nombreArchivo)
            let estadoCargado = JsonSerializer.Deserialize<State>(json)
            { estadoCargado with ProgramState = Running; RedibujarPantalla = true }
        with
        | _ -> estadoInicial
    else
        estadoInicial