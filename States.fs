module Game.States 
open System 
open Game.types

type Misil = {
    X: int
    Y: int
}
type SpecialAbilityCoordinates = {
    x:int 
    y:int
}
type State = {
    ProgramState: ProgramState
    AlienX: int
    AlienY: int
    AlienState: SpriteState
    AlienLives: int
    RedibujarPantalla: bool
    Tick: int
    Clock: int
    Puntuacion: int
    Misiles: Misil list
    EnemigoX: int
    EnemigoY: int
    EnemigoDir: int
    EnemigoEstado: SpriteState
    MisilesEnemigos: Misil list
    EnemigosDerrotados: int
    ColisionAlien: int
    ColisionEnemigo: int
}

let estadoInicial = {
    ProgramState = Running
    AlienX = Console.BufferWidth/2
    AlienY = Console.BufferHeight/2
    AlienState = Alive
    AlienLives = 3
    RedibujarPantalla = true
    Tick = -1
    Clock = 0
    Puntuacion = 0
    Misiles = []
    EnemigoX = Console.BufferWidth-2
    EnemigoY = 0
    EnemigoDir = 1
    EnemigoEstado = Alive
    MisilesEnemigos = []
    EnemigosDerrotados = 0
    ColisionAlien = 0
    ColisionEnemigo = 0
}


// Menu States 


type State<'C> = {
    MenuState: MenuState
    X: int
    Y: int
    CurSorSelection: int
    CursorX: int
    Commands: ('C * string) array
    RedrawScreen: bool
}

let initialState x y commands = 
    {
        MenuState = Active
        X = x
        Y = y
        CurSorSelection = 0
        CursorX = x-2
        Commands = commands
        RedrawScreen = true
    }
