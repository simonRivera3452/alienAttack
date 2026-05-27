module Game.Save 

open System
open System.IO
open System.Text.Json
open System.Text.Json.Serialization
open Game.types
open Game.States

let options = 
    JsonFSharpOptions.Default()
        .ToJsonSerializerOptions()

let rutaBase = AppDomain.CurrentDomain.BaseDirectory
let nombreArchivo = Path.Combine(rutaBase, "record.json")

let guardarPartida (state: State) =
    try
        let json = JsonSerializer.Serialize(state, options)
        File.WriteAllText(nombreArchivo, json)
    with 
    | ex -> printfn "Error al guardar" 


let cargarPartida () =
    if File.Exists nombreArchivo then
        try
            let json = File.ReadAllText(nombreArchivo)
            let estadoCargado = JsonSerializer.Deserialize<State>(json, options)
            { estadoCargado with ProgramState = Running; RedibujarPantalla = true }
        with
        | _ -> estadoInicial
    else
        estadoInicial

        