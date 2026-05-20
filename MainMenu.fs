module Game.mainMenu 

open System
open Game.letters
open Game.Utils
open Game.States
open Game.types

let drawMenu state =
    let logo = MainMenuName()

    // 1. Medimos el logo eliminando los espacios invisibles del final (.TrimEnd())
    let logoWidth = 
        if logo.Length > 0 then 
            logo 
            |> Array.map (fun line -> line.TrimEnd().Length) 
            |> Array.max 
        else 
            1
            
    let logoX = max 0 ((Console.BufferWidth / 2) - (logoWidth / 2))

    logo |> Array.iteri (fun i line ->
        // Al dibujar también limpiamos la línea por si acaso
        displayMessage logoX (2 + i) ConsoleColor.Green (line.TrimEnd())
    )

    // 2. Buscamos la opción más larga del menú de forma automática
    let maxOptionWidth = 
        state.Commands 
        |> Array.map (fun (_, legend) -> legend.Length) 
        |> Array.max
        
    let optionsX = (Console.BufferWidth / 2) - (maxOptionWidth / 2)

    state.Commands
    |> Array.iteri (fun i (_, legend) ->
        displayMessage optionsX (state.Y + i) ConsoleColor.Cyan legend
    )

    // 3. El cursor se alinea al lado de la opción
    displayMessage (optionsX - 4) (state.Y + state.CurSorSelection) ConsoleColor.Yellow ">"

let updateMenuKeyboard (keyInfo: ConsoleKeyInfo) state =
    let key = keyInfo.Key
    let newState =
        match key with 
        | ConsoleKey.UpArrow -> {state with CurSorSelection = max 0 (state.CurSorSelection-1)}
        | ConsoleKey.DownArrow -> {state with CurSorSelection = min (state.Commands.Length-1) (state.CurSorSelection+1)}
        | ConsoleKey.Enter -> {state with MenuState = Terminated}
        | _ -> state

    if newState <> state then 
        {newState with RedrawScreen = true}
    else
        state

// Loop 
let myLoop state = 
    createMainLoop 
        [||]
        (fun s -> s.MenuState = Active) 
        [|updateMenuKeyboard|]
        [| drawMenu|]
        (fun s -> s.RedrawScreen)
        (fun s -> {s with RedrawScreen=false})
        state

let mostrarMainMenu x y commands =
    let oldForeground = Console.ForegroundColor
    Console.CursorVisible <- false

    let state =
        initialState x y commands
        |> myLoop
        
    Console.CursorVisible <- true
    Console.ForegroundColor <- oldForeground
    Console.Clear()
    fst state.Commands[state.CurSorSelection]