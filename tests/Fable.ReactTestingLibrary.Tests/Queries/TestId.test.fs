﻿module TestIdTests

open Fable.Jester
open Fable.ReactTestingLibrary
open Feliz

let testIdElement = React.functionComponent (fun () ->
    Html.div [
        prop.testId "custom-element"
        prop.text "username"
    ])
    
let otherTestIdElement = React.functionComponent (fun () ->
    Html.div [
        prop.testId "other-element"
        prop.text "somethingElse"
    ])

Jest.describe("*ByTestId query tests", fun () ->
    Jest.test("getByTestId an element", fun () ->
        let actual = RTL.render(testIdElement()).getByTestId("custom-element")
            
        Jest.expect(actual).toBeInTheDocument()
        Jest.expect(actual).toHaveTextContent("username")
    )
    Jest.test("getByTestId throws when no element matches", fun () ->
        let actual () = RTL.render(otherTestIdElement()).getByTestId("custom-element")
            
        Jest.expect(actual).toThrow()
    )

    Jest.test("getAllByTestId an element", fun () ->
        let actual = RTL.render(testIdElement()).getAllByTestId("custom-element")
        
        Jest.expect(actual).toHaveLength(1)
        Jest.expect(actual.Head).toHaveTextContent("username")
    )
    Jest.test("getAllByTestId throws when no element matches", fun () ->
        let actual () = RTL.render(otherTestIdElement()).getAllByTestId("custom-element")
            
        Jest.expect(actual).toThrow()
    )

    Jest.test("queryByTestId an element", fun () ->
        let actual = RTL.render(testIdElement()).queryByTestId("custom-element")
        
        Jest.expect(actual).toBeInTheDocument()
        Jest.expect(actual).toHaveTextContent("username")
    )
    Jest.test("queryByTestId no element matches", fun () ->
        let actual = RTL.render(otherTestIdElement()).queryByTestId("custom-element")
            
        Jest.expect(actual).toBeNull()
    )

    Jest.test("queryAllByTestId an element", fun () ->
        let actual = RTL.render(testIdElement()).queryAllByTestId("custom-element")
        
        Jest.expect(actual).toHaveLength(1)
        Jest.expect(actual.Head).toHaveTextContent("username")
    )
    Jest.test("queryAllByTestId no element matches", fun () ->
        let actual = RTL.render(otherTestIdElement()).queryAllByTestId("custom-element")
            
        Jest.expect(actual).toHaveLength(0)
    )

    // Jest.test("findByTestId an element", promise {
    //     let actual = RTL.render(testIdElement()).findByTestId("custom-element")
    //         
    //     do! Jest.expect(actual).resolves.toBeInTheDocument()
    //     do! Jest.expect(actual).resolves.toHaveTextContent("username")
    // })
    // Jest.test("findByTestId throws when no element matches", promise {
    //     let actual = RTL.render(otherTestIdElement()).findByTestId("custom-element")
    //     
    //     do! Jest.expect(actual).rejects.toThrow()
    // })
    //
    // Jest.test("findAllByTestId an element", promise {
    //     let actual = RTL.render(testIdElement()).findAllByTestId("custom-element")
    //         
    //     do! Jest.expect(actual).resolves.toHaveLength(1)
    //
    //     let! actual = actual
    //
    //     do Jest.expect(actual.Head).toHaveTextContent("username")
    // })
    // Jest.test("findAllByTestId no element matches", promise {
    //     let actual = RTL.render(otherTestIdElement()).findAllByTestId("custom-element")
    //         
    //     do! Jest.expect(actual).rejects.toThrow()
    // })
)
