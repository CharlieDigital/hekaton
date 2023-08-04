# Hekaton (WIP)

> [T]he [Hecatoncheires](https://en.wikipedia.org/wiki/Hecatoncheires) were three monstrous giants, of enormous size and strength, each with fifty heads and one hundred arms

## Intro

> ℹ️ This project is a work in progress.

Hekaton is a CLI performance testing tool that uses scenarios defined in YAML.

After searching for easy to use, lightweight open source tools for performance testing, I found that most tools fell into two categories:

1. Too basic; focused on raw load testing
2. Too complex; difficult to adopt for small to medium sized teams

Hekaton is designed for the middle ground with a focus on simplicity (define test scenarios in a YAML file) while being more than just a simple load testing tool (model user journeys).

It is built around the concept of `Scenario`s  which represent the actions in a typical user journey through a system.  Rather than focusing on raw RPS, it's designed to allow teams to model real-world user journeys.

## Objectives

- Easy to model user journeys using simple YAML manifests
- Test definition stored as part of source control and executed remotely triggered from CI
- Support running multi-step test scenarios representing user journeys in parallel
- Support complex scenarios including performing authentication and processing of HTTP responses to extract parameters (headers, cookies)
- Concurrent processing of requests, responses, and results
- Pluggable output architecture to allow publishing results to console, S3, GitHub issue (as comment), etc.
- Support comparison of performance metrics over time to identify deviations
- Distributed processing of requests to simulate traffic from different origins