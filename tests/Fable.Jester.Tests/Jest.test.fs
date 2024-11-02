﻿module Tests

open Fable.Jester

[<RequireQualifiedAccess>]
module Async =
    let map f computation =
        async {
            let! res = computation
            return f res
        }

let myPromise = promise { return 1 + 1 }
    
let myAsync = async { return 1 + 1 }

Jest.describe("can run basic tests", fun () ->
    Jest.test("running a test", fun () ->
        Jest.expect(1+1).toEqual(2)
    )

    // Jest.test("running a promise test", promise {
    //     do! Jest.expect(myPromise).resolves.toEqual(2)
    //     do! Jest.expect(myPromise |> Promise.map ((+) 1)).resolves.toEqual(3)
    // })

    Jest.test("running an async test", async {
        do! Jest.expect(myAsync).toEqual(2)
        do! Jest.expect(myAsync |> Async.map ((+) 1)).toEqual(3)
    })
)

Jest.describe("how to run a test like test.each", fun () ->
    Jest.test("same functionality as test.each", fun () ->
        for (input, output) in [|(1, 2);(2, 3);(3, 4)|] do 
            Jest.expect(input + 1).toEqual(output)
    )
)

Jest.describe("how to run a describe like describe.each", fun () ->
    let myTestCases = [
        (1, 1, 2)
        (1, 2, 3)
        (2, 1, 3)
    ]

    for (a, b, expected) in myTestCases do
        Jest.test(sprintf "%i + %i returns %i" a b expected, fun () ->
            Jest.expect(a + b).toBe(expected)    
        )
)

Jest.describe("tests with the skip modifier don't get run", fun () ->
    Jest.test.skip("this should skip", fun () ->
        Jest.expect(true).toEqual(false)
    )

    Jest.test.skip("this should skip", async {
        do Jest.expect(true).toEqual(false)
    })

    Jest.test("this should execute", fun () ->
        Jest.expect(true).toEqual(true)
    )
)

Jest.describe("todo tests give us our todo", fun () ->
    Jest.test.todo "Do this!"
)

Jest.describe.skip("these shouldn't run", fun () ->
    Jest.test("this shouldn't run", fun () ->
        Jest.expect(true).toEqual(false)
    )
    Jest.test("this shouldn't run either", fun () ->
        Jest.expect(true).toEqual(false)
    )
)

let mutable afterA = false
let mutable afterE = false
let mutable beforeA = false
let mutable beforeE = false

Jest.describe("before/after async functions", fun () ->
    Jest.test("values aren't set yet", fun () ->
        Jest.expect(afterA).toBeFalsy()
        Jest.expect(afterE).toBeFalsy()
        Jest.expect(beforeA).toBeFalsy()
        Jest.expect(beforeE).toBeFalsy()
    )

    Jest.describe("before/after wrapper", fun () ->
        Jest.afterAll(promise {
            afterA <- true
        })

        Jest.afterEach(async {
            afterE <- true
        })

        Jest.beforeAll(promise {
            beforeA <- true
        })

        Jest.beforeEach(async {
            beforeE <- true
        })

        Jest.test("before values are now set", fun () ->
            Jest.expect(beforeA).toBeTruthy()
            Jest.expect(beforeE).toBeTruthy()
        )

        Jest.test("afterAll not called yet", fun () ->
            Jest.expect(afterA).toBeFalsy()
        )

        Jest.test("afterEach is now set", fun () ->
            Jest.expect(afterE).toBeTruthy()
        )
    )   

    Jest.test("after values are now set", fun () ->
        Jest.expect(afterA).toBeTruthy()
        Jest.expect(afterE).toBeTruthy()
    )
)
