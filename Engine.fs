module Game.Engine
open Game.UpdateFunctionsInGame
//
// Juego from scratch
//
open System
open System.Threading
open Game.States
open Game.types




let rec mainLoop state =
    state
    |> actualizarTick
    |> updateClock
    |> actualizarMisiles
    |> actualizarEnemigo
    |> actualizarDisparoEnemigo
    |> actualizarMisilesEnemigos
    |> detectarColisionConAlien
    |> detectarColisionConEnemigo
    |> resetAlien
    |> resetEnemigo
    |> procesarTeclado
    |> redibujarPantalla
    |> fun nuevoEstado ->
        if nuevoEstado.ProgramState = Finished then
            nuevoEstado
        else
            Thread.Sleep 25
            nuevoEstado |> mainLoop

Console.Clear()
Console.CursorVisible <- false

let mostrarGame estadoEntrada =
    let game =
        Console.CursorVisible <- false
        estadoEntrada 
        |> mainLoop
    game



