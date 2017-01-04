module GameDownloader
    #load "AsyncEventQueue.fsx"

    open System.Net
    open System.Threading
    open AsyncEventQueue

    let q = AsyncEventQueue.instance

    let rec ready() =
        async {
             let! msg = q.Receive()
             match msg with
             | Download url -> return! loading(url)
             | Clear     -> return! ready()
             | _         -> failwith("ready: unexpected message")
         }

     and loading(url) =
        async {
            use ts = new CancellationTokenSource()

            // start the load
            Async.StartWithContinuations (
                async {
                    let webCl = new WebClient()
                    let! html = webCl.AsyncDownloadString(Uri url)
                    return html
                 },
                (fun html -> q.Post (Web html)),
                (fun _ -> q.Post Error),
                ts.Token
            )

            disable [startButton; clearButton]
            let! msg = ev.Receive()
            match msg with
            | Web html ->
              let ans = "Length = " + String.Format("{0:D}",html.Length)
              return! finished(ans)
            | Error   -> return! finished("Error")
            | Cancel  -> ts.Cancel()
                       return! cancelling()
            | _       -> failwith("loading: unexpected message")
        }
