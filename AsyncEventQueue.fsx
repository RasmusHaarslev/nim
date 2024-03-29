open System
open System.Threading

type AsyncEventQueue<'T>() =
    let mutable cont = None
    let queue = System.Collections.Generic.Queue<'T>()
    let tryTrigger() =
        match queue.Count, cont with
        | _, None -> ()
        | 0, _ -> ()
        | _, Some d ->
            cont <- None
            d (queue.Dequeue())

    let tryListen(d) =
        if cont.IsSome then invalidOp "multicast not allowed"
        cont <- Some d
        tryTrigger()

    member x.Post msg = queue.Enqueue msg; tryTrigger()
    member x.Receive() =
        Async.FromContinuations (fun (cont,econt,ccont) ->
            tryListen cont)


type Message =
    | NewGame
    | Take
    | Write of String
    | SelectHeap of int
    | Menu
    | NewAiGame


let (instance: AsyncEventQueue<Message>) = AsyncEventQueue()
