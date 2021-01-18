# DeFi-csp-models

The repository contains the source code of a [Curve Compound Pool](https://www.curve.fi/compound/) model defined in CSP# for [PAT](https://pat.comp.nus.edu.sg/). 

```Compound.csp``` contains a CSP# implementation of the [cUSDC](https://etherscan.io/address/0x39aa39c021dfbae8fac545936693ac917d5e7563) Compound smart contract.

```Curve_cPool.csp``` contains a CSP# definition of [Curve.fi: Compound Deposit](https://etherscan.io/address/0xeb21209ae4c2c9ff2a86aca31e123764a3b6bc06) and [Curve.fi: Compound Swap](https://etherscan.io/address/0xa2b47e3d5c44877cca798226b7b8118f9bfb7a56) smart contracts.

```Tokens.csp``` is a CSP# implementation of [USDC](https://etherscan.io/address/0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48) and [cCrv (cDAI+cUSDC)](https://etherscan.io/address/0x845838df265dcd2c412a1dc9e959c7d08537f8a2) tokens.

```Model.csp``` describes behaviors of Curve and Compound users, the analyzed system, and verified properties for both Curve and Compound protocols.

```Pat.Lib.Curve.cs``` implements some of the Curve and Compound mathematical computations in C#.
