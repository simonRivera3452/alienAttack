module Game.Engine
open Game.Utils
open System


type ProgramState = 
| Running
| Terminated

type State = {
    ProgramState: ProgramState
    x: int
    y: int
    RedrawScreen: bool
}

let initialState = {
    ProgramState = Running
    x = Console.BufferWidth/2
    y = Console.BufferHeight/2
    RedrawScreen = true
}

let displayMonster state =
    displayMessage state.x state.y ConsoleColor.Yellow "👽"

let updateMonsterKeyboard (keyInfo: ConsoleKeyInfo)  state =
    let key = keyInfo.Key //
    let newState =
        match key with 
        | ConsoleKey.UpArrow -> {state with y= max 0 (state.y-1)}
        | ConsoleKey.DownArrow -> {state with y = min (Console.BufferHeight-1) (state.y+1)}
        | ConsoleKey.LeftArrow -> {state with x = max 0 (state.x-1)}
        | ConsoleKey.RightArrow -> {state with x = min (Console.BufferWidth-2) (state.x+1)}
        | ConsoleKey.Escape -> {state with ProgramState = Terminated}
        | _ -> state
    if state <> newState then
        {newState with RedrawScreen = true}
    else
        state 


// loop 

let myLoop state = 
    createMainLoop 
        [| |] 
        (fun s -> s.ProgramState = Running) 
        [| updateMonsterKeyboard |]
        [| displayMonster|]
        (fun s -> s.RedrawScreen)
        (fun s -> {s with RedrawScreen=false})
        state

let mostrar() =
    Console.Clear()
    Console.CursorVisible <- false

    initialState
    |> myLoop
    |> ignore

    Console.CursorVisible <- true
    Console.Clear()