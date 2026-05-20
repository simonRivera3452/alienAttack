module Game.States 
open System 
open Game.types
type State = {
    ProgramState: ProgramState
    AlienX: int
    AlienY: int
    AlienState: SpriteState
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
    ColisionAlien: int
    ColisionEnemigo: int
}

let estadoInicial = {
    ProgramState = Running
    AlienX = Console.BufferWidth/2
    AlienY = Console.BufferHeight/2
    AlienState = Alive
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
    ColisionAlien = 0
    ColisionEnemigo = 0
}
