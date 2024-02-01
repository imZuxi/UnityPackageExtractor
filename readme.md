# UnityPackage Extractor

## Purpose

The **UnityPackage Extractor** was created with the following goal in mind:

**Security Considerations**: Unity packages may contain various assets, scripts, and potentially harmful elements. This tool enables users to inspect the contents without exposing their system to potential threats.

## Security Concerns

When extracting Unity packages, it is crucial to be aware of potential security risks, especially from less reputable sources. Some points to consider:

- **Unity Run-On Attribute**: Malicious Unity packages may attempt to execute code on extraction. This tool helps users inspect for any attributes or files that might execute code upon extraction.

- **Malware Prevention**: Extracting Unity packages from untrusted sources could introduce malware. Users should exercise caution and only extract packages from reliable and trusted sources.

---

**Disclaimer**: This tool is provided with the intent of aiding in legitimate use cases such as extracting specific files and ensuring security. Users are advised to exercise caution and follow best practices when dealing with Unity packages from unknown or untrusted sources