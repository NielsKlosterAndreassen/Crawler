module Crawler
    open System
    open HtmlAgilityPack
    open System.IO
    open System.Xml

    let requestPage (url : Uri) =
        let req = System.Net.WebRequest.Create(url)
        let resp = req.GetResponse()
        let stream = resp.GetResponseStream()
        let reader = new StreamReader(stream)
        let data = reader.ReadToEnd()
        resp.Close()
        data

    let getLink (node : HtmlNode) (baseUrl : Uri) =
        let attribute = node.GetAttributeValue ("href", "no url")
        new Uri(baseUrl, attribute)

    let getLinks url =
        let document = new HtmlDocument()
        document.LoadHtml (requestPage url)
        let links = document.DocumentNode.SelectNodes "//a[@href]"
        match links with
            | null -> Seq.empty
            | _ ->
                links
                |> Seq.map (fun node -> getLink node (new Uri("http://" + url.Host)))

    let rec crawlPage (url : Uri) pagesVisited =
        let links =
            getLinks url
            |> Seq.filter (fun link -> not (Seq.exists (fun x -> x = link) pagesVisited))
            |> Seq.distinct
        let pagesVisitedNow = Seq.append pagesVisited links
        links
            |> Seq.collect (fun x -> crawlPage x pagesVisitedNow)

    [<EntryPoint>]
    let main argv =
        let result : seq<Uri> = crawlPage (new Uri(argv.[0])) Seq.empty
        printfn "Successfully crawled %d pages" (Seq.length result)
        let input = System.Console.ReadLine();
        0
