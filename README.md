# 🚛 OmniPulse - Enterprise Industrial Fleet & Cold-Chain Smart Telemetry Platform

<p align="center">
  <img src="https://img.shields.io/badge/version-v3.0_Titan_Apex-blue?style=for-the-badge" alt="Version" />
  <img src="https://img.shields.io/badge/.NET-10.0_LTS-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="Framework" />
  <img src="https://img.shields.io/badge/Database-InfluxDB_3.0_Flux-00C853?style=for-the-badge&logo=influxdb" alt="Time-Series" />
  <img src="https://img.shields.io/badge/Realtime-SignalR_Redis-d63939?style=for-the-badge" alt="WebSockets" />
  <img src="https://img.shields.io/badge/license-Non--Commercial%20Portfolio-red?style=for-the-badge" alt="License" />
  <img src="https://img.shields.io/badge/Production_Ready_Showcase-✓-00C853?style=for-the-badge" alt="Status" />
</p>

<p align="center">
  <b>Mission-Critical Cold-Chain Telemetry & High-Frequency Fleet Monitoring Architecture</b><br>
  <i>A Technical Showcase of Enterprise Software Engineering & Glass-Box System Architecture</i>
</p>

---

## 📋 Executive Summary

OmniPulse is an advanced, high-throughput industrial fleet telemetry and cold-chain monitoring platform engineered with **.NET 10**. Designed specifically to demonstrate **Enterprise-Level System Architecture**, it simulates 150+ refrigerated vehicles and IoT sensors streaming real-time thermal (+2°C to +8°C), spatial (GPS/Speed), and diagnostic data.

Unlike basic CRUD applications, OmniPulse tackles critical distributed engineering challenges such as **high-frequency concurrent data ingestion without database locking**, **zero-trust security encapsulation**, **time-series analytics**, and **automated incident workflows**.

---

## 🏗️ Core Architectural Capabilities & Tech Stack

```text
┌────────────────────────────────────────────────────────────────────────┐
│                      Presentation Layer (Tabler UI)                    │
│  - Executive Fleet Dashboard         - Time-Series Telemetry Panel     │
│  - Global Audit Forensics            - Anomaly Alert War Room          │
│  - Microservice Topology Monitor                                       │
├────────────────────────────────────────────────────────────────────────┤
│                   Asynchronous Channel Buffer                          │
│  - System.Threading.Channels<T>      - Zero-Locking Concurrency        │
├────────────────────────────────────────────────────────────────────────┤
│                     Background Processing Engine                       │
│  - TelemetryIngestionWorker          - SignalR Real-Time WebSocket Push│
│  - Elsa Workflows Automation         - MCP Agentic AI Reasoning Chain  │
├────────────────────────────────────────────────────────────────────────┤
│                   Dual-Database Data Access Layer                      │
│  - InfluxDB 3.0 (Time-Series)        - SQL Server 2022 (Relational)    │
│  - EF Core Interceptors (Audit)      - Global Query Filters (SoftDel)  │
└────────────────────────────────────────────────────────────────────────┘
```
<table>
  <thead>
    <tr>
      <th>Area</th>
      <th>Technology</th>
      <th>Purpose / Implementation</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><b>Framework</b></td>
      <td><b>.NET 10.0 (C# 12/13)</b></td>
      <td>Core Web API &amp; Application Pipeline</td>
    </tr>
    <tr>
      <td><b>Time-Series DB</b></td>
      <td><b>InfluxDB 3.0 / Flux</b></td>
      <td>High-Frequency Telemetry Ingestion &amp; Storage</td>
    </tr>
    <tr>
      <td><b>Concurrency</b></td>
      <td><b>System.Threading.Channels</b></td>
      <td>Thread-Safe In-Memory Unbounded Queue</td>
    </tr>
    <tr>
      <td><b>Real-Time WebSockets</b></td>
      <td><b>SignalR + Redis Backplane</b></td>
      <td>Millisecond Live Chart.js Data Streaming</td>
    </tr>
    <tr>
      <td><b>Security &amp; Auth</b></td>
      <td><b>Keycloak OIDC / Zero-Trust</b></td>
      <td>Encapsulated Claims &amp; QuantumSafeMinter Signatures</td>
    </tr>
    <tr>
      <td><b>Workflow Engine</b></td>
      <td><b>Elsa Workflows</b></td>
      <td>Inheritance-Based Automated Incident Dispatch</td>
    </tr>
    <tr>
      <td><b>Agentic AI</b></td>
      <td><b>MCP Server (Model Context)</b></td>
      <td>Polymorphic Telemetry Anomaly Prediction</td>
    </tr>
    <tr>
      <td><b>ORM &amp; Audit</b></td>
      <td><b>EF Core 8/10 + Interceptors</b></td>
      <td>Automatic Audit Logging &amp; Soft Delete Filtering</td>
    </tr>
  </tbody>
</table>

<br>

---

## 🧩 OOP Architectural Mapping

OmniPulse explicitly demonstrates the 4 fundamental pillars of Object-Oriented Programming within its core modules:

* 🛡️ **Encapsulation (Kapsülleme):** Security claims and JWT token validation are encapsulated within `TitanZeroTrustHandler` and `QuantumSafeMinter`. Thread-safe channels encapsulate internal queue state in `TelemetryChannel.cs`.
* 📐 **Abstraction (Soyutlama):** Data access is abstracted behind `ITelemetryRepository` and `ITelemetryChannel`, ensuring complete DB-agnostic flexibility (relational SQL vs time-series InfluxDB).
* 🧬 **Inheritance (Kalıtım):** Automation workflow steps derive from the abstract `BaseWorkflowStep` parent class. Background workers inherit from .NET `BackgroundService`.
* 🎭 **Polymorphism (Çok Biçimlilik):** `IMcpAgent` and `IDeviceCommunicator` interfaces invoke dynamic, context-specific mitigation strategies for different telemetry sensor protocols (MQTT, CAN-Bus, Modbus).

<br>

---

## ⚠️ Legal Notice & Non-Commercial Usage Rights

© 2026 Doruk AVGIN. All Rights Reserved.

This software is an independent personal portfolio project developed strictly for educational, architectural demonstration, and career evaluation purposes.

### ✅ Permitted Use
* Viewing, studying, and reviewing the source code for education and recruitment evaluation.
* Running the application locally for technical assessment and code review.

### ❌ Restricted Use
* **NO Commercial Use:** This software, in whole or in part, may NOT be used for commercial activities, corporate deployments, or revenue-generating services without explicit written consent.
* **NO Redistribution:** You may not resell, white-label, or package this software into commercial products.

<br>

---

## 👤 Author & Contact

**Doruk AVGIN**  
*Electrical & Electronics Engineer | Senior Full-Stack .NET Developer*

* 📍 **Location:** Ankara, Turkiye  
* 🔗 **LinkedIn:** [linkedin.com/in/dorukavgin](https://www.linkedin.com/in/dorukavgin)  
* 💻 **GitHub:** [github.com/doruk-developer](https://github.com/doruk-developer)  
* 🌐 **Portfolio Showcase:** [curelogix.com.tr](https://www.curelogix.com.tr)