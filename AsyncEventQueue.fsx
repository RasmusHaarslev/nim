module AsyncEventQueue
    open System
    open System.Threading

    // An asynchronous event queue kindly provided by Don Syme
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

    // An enumeration of the possible events
    type Message = | Move of int*int | Error | SelectHeap of int | NewGame | NewAiGame | Clear
    // Defining an insatnce here works, but we should investigate if there's
    // a cool way of doing singletons!!
    let (instance: AsyncEventQueue<Message>) = AsyncEventQueue()
