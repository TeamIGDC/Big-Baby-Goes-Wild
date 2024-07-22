# Contributing to Big Baby Goes Wild

If you're reading this, it's likely because you're interested in contributing to the game's development. That's awesome, and it means a lot <3. However, there are a few housekeeping things you should know about.

## Developer Certificate of Origin

When contributing code to the game, you must sign the the Developer Certificate of Origin. This certifies that you wrote the code you're submitting, or otherwise have permission to submit it in the first place. When creating pull requests, all commits must contain a `Signed-off-by: YOUR NAME <YOUR EMAIL>` line as an indication that you have read and understand the Developer Certificate of Origin. Your work will not be merged unless this check passes.

The Developer Certificate of Origin is presented below. You may also obtain a verbatim copy of this document at https://developercertificate.org/.

```
Developer Certificate of Origin
Version 1.1

Copyright (C) 2004, 2006 The Linux Foundation and its contributors.

Everyone is permitted to copy and distribute verbatim copies of this
license document, but changing it is not allowed.


Developer's Certificate of Origin 1.1

By making a contribution to this project, I certify that:

(a) The contribution was created in whole or in part by me and I
    have the right to submit it under the open source license
    indicated in the file; or

(b) The contribution is based upon previous work that, to the best
    of my knowledge, is covered under an appropriate open source
    license and I have the right under that license to submit that
    work with modifications, whether created in whole or in part
    by me, under the same open source license (unless I am
    permitted to submit under a different license), as indicated
    in the file; or

(c) The contribution was provided directly to me by some other
    person who certified (a), (b) or (c) and I have not modified
    it.

(d) I understand and agree that this project and the contribution
    are public and that a record of the contribution (including all
    personal information I submit with it, including my sign-off) is
    maintained indefinitely and may be redistributed consistent with
    this project or the open source license(s) involved.
```

By contributing to the game, you also agree that your code can and will be distributed under the terms of the MIT License (see `/LICENSE`), and thus will be included in the paid version of the game with attribution.

## Pull request format

Pull requests must follow the below formatting rules

### Squashing

Please squash your commits! Dealing with one single large commit is much better than many tiny commits. This helps us track down bugs and regressions to the exact contribution they came from.

### Commit message format

When writing a commit message, use the following example as a template. Commits should summarize what they're accomplishing and why, as well as what's being changed. If relevant, they must also indicate what issues they resolve.

```
Allow players to be able to choose between different gods

This change allows the players to either use a sword or hand combat.

- Add the the ability to do hand combats
- Add player's ability to switch from sword to punches

Fixes issue #226
Signed-off-by: YOUR NAME <YOUR EMAIL ADDRESS>
```

### Branches

Please do not commit directly to `master`. The `master` branch represents the current stable release of the game.

Development of the next version of the game is done on a `work/VERSION` branch. Only team members may merge into these branches.

Only team members may create branches under the `work/` prefix.

The `devel` branch is identical to `master`, but includes outside contributions destined for the next version of the game. The `work/VERSION` branches are rebased onto it.

For all contributions,

1. Fork the project if you haven't already
2. When starting a new contribution, create a new branch to work inside
3. Name it after the feature you're working on, e.g. `accessibility-fixes`, `my-awesome-feature`
4. Commit changes to that branch on your fork
5. Squash all of your commits into one, using the template above for the messages
6. Create a pull request on this repository, requesting to merge your branch into the `devel` branch.

### Code review and code style

All code contributions must look like it belongs in the game's codebase.

All code contributions must pass a code review before merging.
