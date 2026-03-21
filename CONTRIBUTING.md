# Contributing to JAAvila.FluentOperations

Thank you for your interest in contributing to JAAvila.FluentOperations. This document outlines the guidelines and expectations for contributors.

## Code of Ethics

### For Contributors

By contributing to this project, you agree to:

1. **Respect the original work.** This project represents years of design and engineering effort. Contributions should enhance, not replace or undermine, the existing architecture.

2. **Attribute properly.** If your contribution is inspired by or derived from another project, library, or resource, clearly state the source in your pull request description.

3. **Act in good faith.** Do not submit contributions with the intent of introducing vulnerabilities, backdoors, or code that degrades the project's quality or security.

4. **Maintain compatibility.** Follow the established architectural patterns (6 Pilares, ExecutionEngine pattern, one-validator-per-operation). Contributions that break these patterns without prior discussion will not be accepted.

5. **Respect the license.** All contributions are submitted under the Apache License 2.0. You must have the legal right to submit the code.

### For Fork Authors

If you fork this project, we ask that you:

1. **Choose a distinct name** for your fork. See [TRADEMARK.md](TRADEMARK.md) for naming guidelines.

2. **Include attribution** as described in the [NOTICE](NOTICE) file. This is not a request — it is required by Section 4(d) of the Apache License 2.0.

3. **Do not misrepresent** your fork as the official project or imply endorsement by the original author.

4. **Consider contributing back.** If you fix a bug or add a feature that would benefit the community, consider submitting a pull request to the upstream project instead of maintaining a permanent fork.

### For Commercial Users

This project is free for commercial use under Apache 2.0. We ask that commercial users:

1. **Comply with the NOTICE file** requirements when distributing the software.
2. **Consider supporting the project** through contributions, bug reports, or public acknowledgment.
3. **Do not rebrand and sell** the project as your own product. The code is free; the brand is not.

## Contributor License Agreement (CLA)

By submitting a pull request, you represent that:

- You have the right to license the contribution under the Apache License 2.0.
- Your contribution does not violate any third-party intellectual property rights.
- You grant the project maintainer (JAAvila) a perpetual, worldwide, non-exclusive, royalty-free license to use, modify, and distribute your contribution as part of the project.
- The project maintainer retains the right to relicense the project (including your contribution) under a different license in the future, provided the Apache 2.0 license for existing versions remains in effect.

## How to Contribute

### Reporting Issues

- Use the [GitHub Issues](https://github.com/JAAvila-Of/JAAvila.FluentOperations/issues) tracker.
- Include: .NET version, FluentOperations version, minimal reproduction code, expected vs actual behavior.

### Submitting Pull Requests

1. Fork the repository and create a feature branch.
2. Follow the coding patterns documented in [CLAUDE.md](CLAUDE.md).
3. Ensure all 6 Pilares are covered for new operations/validators.
4. Every new validator MUST implement both `IValidator` and `IRuleDescriptor`.
5. Run the full test suite: `dotnet test JAAvila.FluentOperations.Testing/JAAvila.FluentOperations.Testing.slnx`
6. Update documentation (`README.md`, `docs/API.md`) as applicable.
7. Use conventional commit messages in English.

### What We Accept

- Bug fixes with regression tests.
- New validators/operations following the established patterns.
- Improvements to error messages or formatting.
- Documentation improvements.
- Performance optimizations with benchmarks.
- Localization translations (`.resx` files).

### What We Do Not Accept

- Changes that break backward compatibility without prior discussion.
- Features that contradict the [Anti-Features](competitive_features_roadmap.md#7-anti-features-what-not-to-build) list.
- Code without tests.
- Pull requests that rename or rebrand the project.

## Recognition

All contributors are recognized in the project. Significant contributions will be acknowledged in release notes and the project's documentation.

---

*By contributing to this project, you agree to abide by this document and the project's [Apache License 2.0](LICENSE), [NOTICE](NOTICE), and [TRADEMARK](TRADEMARK.md) policies.*
