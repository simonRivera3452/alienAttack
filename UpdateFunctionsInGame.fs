module Game.UpdateFunctionsInGame
open System
open Game.types
open Game.States
open Game.DisplayObjectsInGame


// redraw
let redibujarPantalla state =
    if state.RedibujarPantalla then 
        Console.Clear()
        [|
            displayClock
            displayPuntuación
            displayAlienLives
            dibujarAlien
            dibujarMisiles
            dibujarEnemigo
            dibujarMisilesEnemigos
        |]
        |> Array.iter (fun f -> f state)
        {state with RedibujarPantalla=false}
    else
        state

// update

let actualizarTick state =
    {state with Tick = state.Tick+1}
let updateClock state =
    if state.Tick <> 0 && state.Tick % 40 = 0 then
        {state with 
            Clock = state.Clock+1
            Puntuacion = state.Puntuacion + 100
        }
    else
        state


let actualizarMisiles state =
    if state.Misiles <> [] then 
        state.Misiles
        |> Seq.map (fun misil -> { misil with X = misil.X + 1})
        |> Seq.filter (fun misil -> misil.X < Console.BufferWidth - 2)
        |> Seq.toList
        |> fun nuevosMisiles ->
            { state with Misiles = nuevosMisiles; RedibujarPantalla = true } 
    else
        state

let actualizarMisilesEnemigos state =
    if state.MisilesEnemigos <> [] then 
        state.MisilesEnemigos
        |> Seq.map (fun misil -> { misil with X = misil.X - 1})
        |> Seq.filter (fun misil -> misil.X >= 0)
        |> Seq.toList
        |> fun nuevosMisiles ->
            { state with MisilesEnemigos = nuevosMisiles; RedibujarPantalla = true } 
    else
        state
   
let actualizarDisparoEnemigo state =
    if state.EnemigoEstado = Alive && state.Tick % 4 = 0 then 
        let nuevoMisil = {
            X = state.EnemigoX - 2
            Y = state.EnemigoY
        }
        {state with MisilesEnemigos = nuevoMisil :: state.MisilesEnemigos; RedibujarPantalla = true}
    else
        state
let actualizarEnemigo state =
    if state.EnemigoEstado = Alive then

        let funcionElegida = Math.Sin
        let amplitud = 12.0     
        let frecuencia = 0.05  

        let centroY = float (Console.BufferHeight / 2)
        
        let desplazamiento = amplitud * funcionElegida (frecuencia * float state.Tick)
        let nuevoY = int (Math.Round(centroY + desplazamiento))
        
        let maxAbajo = Console.BufferHeight - 2
        let Yfinal = nuevoY |> max 0 |> min maxAbajo
        
        { state with EnemigoY = Yfinal; RedibujarPantalla = true }
    else
        state

// detect collisions
let detectarColisionConAlien state =

    if state.AlienState = Hit then 
        state
    else
        state.MisilesEnemigos
        |> List.filter (fun misil -> not (misil.X = state.AlienX+1 && misil.Y = state.AlienY))
        |> fun nuevosMisiles ->
            if nuevosMisiles.Length <> state.MisilesEnemigos.Length then 
                { state with 
                    AlienState = Hit
                    MisilesEnemigos = nuevosMisiles
                    RedibujarPantalla = true
                    ColisionAlien = state.Tick
                    AlienLives = max 0 (state.AlienLives - 1)
                }
            else
                state
let detectarColisionConEnemigo state =

    if state.EnemigoEstado = Hit then 
        state
    else
        state.Misiles
        |> List.filter (fun misil -> not (misil.X = state.EnemigoX-1 && misil.Y = state.EnemigoY))
        |> fun nuevosMisiles ->
            if nuevosMisiles.Length <> state.Misiles.Length then 
                { state with 
                    EnemigoEstado = Hit
                    Misiles = nuevosMisiles
                    RedibujarPantalla = true
                    ColisionEnemigo = state.Tick
                    Puntuacion = state.Puntuacion + 250
                    EnemigosDerrotados = state.EnemigosDerrotados + 1
                }
            else
                state

// resets state after hit
let resetAlien state =
    if state.AlienState = Hit then 
        let tiempo = state.Tick-state.ColisionAlien
        if tiempo >= 80 then 
            {state with AlienState=Alive;RedibujarPantalla=true}
        else
            state
    else
        state

let resetEnemigo state =
    if state.EnemigoEstado = Hit then 
        let tiempo = state.Tick-state.ColisionEnemigo
        if tiempo >= 80 then 
            {state with EnemigoEstado=Alive;RedibujarPantalla=true}
        else
            state
    else
        state

// process keyboard input
let procesarEscape key state =
    match key with 
    | ConsoleKey.Escape ->
        {state with ProgramState = Finished}
    | _ -> state
let procesarTecladoAlien key state =
    if state.AlienState = Alive then 
        match key with 
        | ConsoleKey.Spacebar ->
            let nuevoMisil = {
                X = state.AlienX + 2
                Y = state.AlienY
                //OrigenY = state.AlienY
                //Distancia = 0 // <--- Nace con distancia 0
            }
        
            {state with Misiles = nuevoMisil :: state.Misiles}
        | ConsoleKey.UpArrow ->
            {state with AlienY = max 0 (state.AlienY-1)}
        | ConsoleKey.DownArrow ->
            {state with AlienY = min (Console.BufferHeight-1) (state.AlienY+1)}
        | ConsoleKey.LeftArrow ->
            {state with AlienX = max 0 (state.AlienX-1)}
        | ConsoleKey.RightArrow ->
            {state with AlienX = min (Console.BufferWidth-2) (state.AlienX+1)}
        | _ -> state
        |> fun nuevoEstado ->
            if nuevoEstado <> state then 
                {nuevoEstado with RedibujarPantalla=true}
            else
                state
    else
        state

let procesarTeclado state =
    if Console.KeyAvailable then 
        let k = Console.ReadKey true
        state
        |> procesarEscape k.Key
        |> procesarTecladoAlien k.Key
    else
        state

let CheckGameOver state =
    if state.AlienLives <= 0 then 
        {state with ProgramState = GameOver; RedibujarPantalla=true}
    else
        state