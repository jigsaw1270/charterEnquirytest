# Bluebnc.com Charter Enquiry Automation Test

## Overview

This repository contains automation tests for the Bluebnc.com charter enquiry system. The primary focus is on verifying basic booking functionality across random destinations and boats. The tests are designed to run on Chromium and are executed using the .NET testing framework.

## Test Description

The automation test is performed as a single task, with the goal of verifying that the booking process works correctly. The test:

- Randomly selects any destination.
- Randomly selects any boat.
- Ensures the booking process completes successfully.

The expected result of the test is a success count of 1, indicating that the test has passed.

## Test Execution

To execute the test, ensure that you have the necessary environment and tools set up. Follow the steps below:

1. **Setup Chromium Browser**

   Ensure Chromium is installed and properly configured on your system.

2. **Run the Test**

   Use the following command to execute the test with the specified run settings:

   ```bash
   dotnet test --settings:demo.runsettings
