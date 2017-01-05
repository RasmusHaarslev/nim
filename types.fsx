module Types

    type AiType =
        | NoAI
        | Easy
        | Medium
        | Hard
        | Godlike

    type Message =
        | Move of int*int
        | Error
        | NewGame of AiType
        | Clear
        | Download of string

    type GameState = {
        TurnBit: bool;
        Heap: int list;
        Ai: AiType
    }