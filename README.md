# Hekaton (WIP)

> [T]he [Hecatoncheires](https://en.wikipedia.org/wiki/Hecatoncheires) were three monstrous giants, of enormous size and strength, each with fifty heads and one hundred arms

## Intro

> ℹ️ This project is a work in progress.  I plan on working on it on and off!

Hekaton is a CLI performance testing tool that uses scenarios defined in YAML.

After searching for easy to use, lightweight open source tools for performance testing, I found that most tools fell into two categories:

1. **Too basic**; focused on raw *load testing* and do not simulate real user flows
2. **Too complex**; designed for dedicated performance testing teams and difficult to adopt for small to medium sized teams

Hekaton is designed for the middle ground with a focus on simplicity (define test scenarios in a YAML file) while being more than just a simple load testing tool (model user journeys).

It is built around the concept of `Scenario`s  which represent the actions in a typical user journey through a system.  Rather than focusing on raw RPS, it's designed to allow teams to model real-world user journeys.

## Sample YAML

```yaml
name: Basic Performance Test
# Base URL to simplify downstream URLs
baseUrl: https://www.example.com

# A series of independent scenarios.  Each scenario represents a logical flow
# or user journey through the system.
scenarios:

# First scenario is an external user browsing the page. Up to 100 users.
- name: User Browsing Landing Page

  vusers:
    initial: 5                          # Start the scenario with 5 users
    max: 100                            # And create up to a total of 100 users
    ramp:
      every: 10s                        # Every 10 seconds
      variation: 0.25                   # With a variation of up to 25%
      add: 1                            # Add 1 user until reaching 10 total

  pause:
    duration: 7s                        # By default, pause 7 seconds after each step
    variation: 0.25                     # With a variation of up to 25%

  steps:

  - name: Load Page
    type: HttpGet                       # The type of the step; default is HttpGet
    url: https://www.example.com        # Browse this URL
    sla:
      p90: 1500                         # Target p90 SLA
      p95: 1600                         # Target P95 SLA
    generates:                          # Generate these additional requests.
    - "/images/logo.png"                # Uses base URL
    - "http://cdn.example.com/static/app.js"

  - name: Navigate Product Detail Page
    type: HttpGet
    url: https://www.example.com/p/123456

# Second scenario is an admin user logging into the back-end.  Up to 5 users.
- name: Admin User Scenario

  delay: 100s                           # Delay the start of this scenario 100 seconds

  vusers:
    initial: 1                          # Start the scenario wit1 user
    max: 5                              # And create up to a total of 5 users
    ramp:
      every: 1m                         # Every 1 minute
      variation: 0.10                   # With a variation of up to 10%
      add: 2                            # Add 2 users until reaching 5

  rows:
    source: "./users.csv"               # Read this CSV as input to each user
    read: InOrder                       # Read the rows in order
    columns:                            # With these columns
    - username
    - password

  pause:
    duration: 5s                        # By default, pause 5 seconds after each step
    variation: 0.15                     # With a variation of up to 15%

  steps:
  - name: Admin User Login
    type: HttpPost
    url: /login

    headers:
      Content-type: application/json    # Include these headers

    # Include this body with variable substitution with the column values from the
    # CSV that contains the rows.
    body: "{ username: ___username, password: ___password }"

    pause:                              # Override the default 5s pause
      duration: 10s
      variation: 0.05

    # Extract values from the response and assign to variables that can be used
    # in subsequent steps in the scope of this virtual user.
    response:
      headers:
        ___auth: Authorization           # Assign the Authorization header to __auth
      cookies:
        ___all: ''                       # A special assignment that contains all cookies
        ___someVar: cookieName           # Assigned a specific cookie

  - name: API Call Post Login
    type: HttpGet
    url: /api/load-user-data?page=0&size=25

    headers:
      Authorization: ___auth             # Include these headers
```

## Sample Output

This screenshot shows a sample of the output.

![Screenshot of console](Assets/console-screenshot.png)

Note that the `user_browsing_landing_page.load_page` p90 is rendered in red.  This indicates that the value is above the specified p90 SLA for this request set a 1900ms.

## Objectives

- Easy to model user journeys using simple YAML manifests
- Test definition stored as part of source control and executed remotely triggered from CI
- Support running multi-step test scenarios representing user journeys in parallel
- Support complex scenarios including performing authentication and processing of HTTP responses to extract parameters (headers, cookies)
- Concurrent processing of requests, responses, and results
- Pluggable output architecture to allow publishing results to console, S3, GitHub issue (as comment), etc.
- Support comparison of performance metrics over time to identify deviations
- Distributed processing of requests to simulate traffic from different origins

## Status

||Feature|
|--|--|
|✓|Parse YAML test manifest|
|✓|Streaming calculation of mean, p90, p95, min, max|
|✓|Write to live console output|
|𐄂|Perform basic HTTP actions|
|𐄂|Configure steps with CSV|
|𐄂|Variable replacement in strings|
|𐄂|Capture headers from HTTP response|
|𐄂|Support scripting using JavaScript and JINT|
|𐄂|Write results to collectors|
|𐄂|Write errors to collectors|
|𐄂|Distributed execution|
|𐄂|Distributed results collection|
|𐄂|Remote execution (send manifest to remote executor)|

## Concepts

|Term|Definition|
|--|--|
|`Test`|A test defines a set of `Scenario`s that model user journeys|
|`Scenario`|A scenario encapsulates a series of steps to execute that represent actions that a user takes through a system.  For example, a scenario in e-commerce might be `browse landing page` → `add to cart` → `log in` → `checkout`|
|`Step`|Each `Scenario` is a collection of `Step`s which represent individual substantive HTTP requests that are generated by the user.  This does not include downloading static assets like images, JS files, CSS files, etc. (unless you want to!)|
|`VUser`|A virtual user represents one user performing the `Scenario`.|
|`Row`|`VUser`s can be generated dynamically or defined in an input CSV file (e.g. usernames, email, passwords, etc.)|
|`Pause`|Simulates how real users behave when interacting with the system.  Users will pause, read, scroll, and make decisions before acting.|

## Why Scenarios

In most applications, there are actions performed by multiple roles within the system.  These different roles perform different actions that have an effect on the performance profile of a system.

For example, a content management system (CMS) will have publishers creating new content and consumers reading the published content.  These users will operate on different frequencies.  Ideally, we can easily model the load where 10% of the users are performing a publish operation while 90% of the users are reading the published content to simulate our actual traffic distribution.

## Statistics

The statistics are computed using [T-Digest](https://www.sciencedirect.com/science/article/pii/S2665963820300403).  [The C# implementation](https://github.com/Cyral/t-digest-csharp) is a port of a reference Java implementation.  This approach provides an approximation of the P-90 without actually storing all of the values required to produce an exact P-90.

## Development

```shell
# Run tests from root
dotnet test

# Run samples
dotnet run -- -f samples/basic.yaml

```