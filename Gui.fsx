module Gui

    #load "AsyncEventQueue.fsx"
    open AsyncEventQueue
    open System.Windows.Forms
    open System.Drawing

    let mainWindow = new Form(Text="Nim Player",Size=Size(520,500))
    // Next line disables resizing of the window
    mainWindow.FormBorderStyle <- FormBorderStyle.FixedSingle

    // Add a panel, to group up all the radio buttons
    let heapPanel = new Panel(Size=Size(500,390),Top=10,Left=10)
    heapPanel.AutoScroll <- true
    heapPanel.BackColor <- Color.LightGray

    // Add the buttons/text fields in the bottom of the screen.
    let newGameButton =
      new Button(Location = Point(30,420), MinimumSize=Size(100,50),
                  MaximumSize = Size(100,50),Text = "New Game")

    let makeDrawButton =
      new Button(Location = Point(380,420), MinimumSize=Size(100,50),
                  MaximumSize = Size(100,50),Text = "Draw!")

    let chooseMatches = 
      new TextBox(Location = Point(150,420),Size = Size(200,25))

    
    (* This list is keeping track of how many heaps are added for render, so
     * that we can correctly track the heap selected by the heap.
     *)
    let mutable heapList = []
    (* Given a list of integers of size n,
     * creates n radiobuttons.
     * The integers in the list is used as label for each button.
     * 
     * fun type: int list -> unit
     *)
    let populateHeapPanel heaps =
        printfn "Populating panel with %d heaps" (List.length heaps)
        let rec inner heaps yPos cnt = 
            match heaps with
            | []        -> ()
            | x::xs     ->
                let rad = new RadioButton(Text=x.ToString(),Top=yPos,Left=30)
                heapList <- heapList @ [(rad, cnt)]
                inner xs (yPos + 20) (cnt + 1)

        in            
            inner heaps 0 0
            

    (* Clears all the radiobuttons from the heap panel
     *
     * fun type: unit -> unit
     *)
    let clearHeapPanel () =
        printfn "Clearing HeapPanel"
        while heapPanel.HasChildren do
            heapPanel.Controls.RemoveAt(0)
        ()



    // Add all the items.
    mainWindow.Controls.Add newGameButton
    mainWindow.Controls.Add chooseMatches
    mainWindow.Controls.Add makeDrawButton

    (* Define and add handlerFunction for newGameButton
     * The handler gets some event type - but it is not used in the example
     * 
     * fun type: System.EventArgs -> unit
     *)
    let newGameHandler _ = 
        printfn "Clicked newGameButton"
        clearHeapPanel ()

    newGameButton.Click.Add(newGameHandler)

    (* Define and add handlerFunction for newGameButton
     * The handler gets some event type - but it is not used in the example
     * 
     * fun type: System.EventArgs -> unit
     *)
    let makeDrawHandler _ =
        printfn "Clicked drawButton"
        
        let rec getSelectedHeap (hl : (RadioButton * int) list)=
            match hl with
            | []    -> -1
            | x::xs -> 
                if (fst x).Checked then
                    (snd x)
                else
                    getSelectedHeap xs
        let chosenHeap = getSelectedHeap heapList
        let numMatches = 
            try
                int chooseMatches.Text
            with
                | _ as d -> 
                    printfn "Could not parse integer: %s" chooseMatches.Text
                    -1

        chosenHeap |> printfn "%A"
        numMatches  |> printfn "%A"

        match chosenHeap,numMatches with
            | -1,_       -> AsyncEventQueue.ev.Post AsyncEventQueue.Error
            | _,-1       -> AsyncEventQueue.ev.Post AsyncEventQueue.Error
            | _,_        -> AsyncEventQueue.ev.Post (Move (chosenHeap, numMatches))

        


    makeDrawButton.Click.Add(makeDrawHandler)
        // Add draw event to eventQueue
        // What instance of eventQueue am I using here?
        // Should I verify user input here? (The more the better? :)

    
    //heapPanel.Controls.RemoveAt(0)

    heapPanel.HasChildren |> printfn "%A"

    heapPanel.HasChildren |> printfn "%A"


    mainWindow.Controls.Add heapPanel


    