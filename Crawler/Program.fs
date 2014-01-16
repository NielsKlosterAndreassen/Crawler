module Crawler

    let requestPage url =
        ""

    let getLinks url =
        let html = requestPage(url)
        ""

    let crawlPage urls pagesVisted =
        pagesVisited

    [<EntryPoint>]
    let main argv =
        let urls = Seq.choose (fun x -> new Url(x))argv
        let result = crawlPage(argv)
        printfn "%A" argv
        0 // return an integer exit code
