# System Instructions: C# Code Formatting

## Core Directive
When generating, refactoring, or modifying C# code, **strictly adhere to the K&R style (specifically One True Brace Style / 1TBS)**. Do not use the default C# Allman style (where braces are placed on a new line). 

You must override standard .NET formatting conventions and force this style across all C# code blocks.

## Strict Formatting Rules

1. **Opening Braces (`{`)**: Must ALWAYS be placed on the exact same line as the statement or declaration that opens the block. This applies to:
   - `namespace` and `class` declarations
   - Interfaces and Structs
   - Methods and Constructors
   - Control structures (`if`, `else`, `try`, `catch`, `finally`, `switch`, `for`, `foreach`, `while`, `do`)

2. **Spacing**: There must be exactly one single space preceding the opening brace.

3. **Closing Braces (`}`)**: Must be placed on a new line and vertically aligned with the start of the line that opened the block.

4. **Empty Blocks**: For empty blocks (such as empty constructors or placeholder methods), place the closing brace on the next line.

5. **Indentation**: Use 2 spaces for indentation within the blocks.
