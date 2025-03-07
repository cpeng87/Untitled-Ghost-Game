import re
def parse_dialogue(file_path):
    ghost_title = file_path.rsplit('.',1)[0]
    ghost_name = file_path.rsplit('.',1)[0] + " Ghost"
    # Read input text from a file
    with open(file_path, 'r', encoding='utf-8') as file:
        input_text = file.read()

    # Split the input into sections based on the [Title] pattern
    sections = re.split(r'\[(.*?)\]', input_text)

    # Remove empty strings from splitting and group into title-content pairs
    sections = [(sections[i], sections[i + 1].strip()) for i in range(1, len(sections) - 1, 2) if sections[i] != "End" and sections[i] != "EndDialogue"]

    # Default position for simplicity
    default_positions = {
        "Order": "-54,-23",
        "Success": "-173,132",
        "Failure": "-112,-180",
        "Story": "169,38",
    }

    # Convert sections into Yarn Spinner format
    yarn_sections = []
    for title, content in sections:
        lines = content.splitlines()
        yarn_title = f"title: {ghost_title + title}"
        tags = "tags:"
        position = f"position: {default_positions.get(title, '0,0')}"

        # Filter out lines that are metadata (e.g., [End])

        body_lines = []
        for line in lines:
            if (line.startswith("[") == False):
                # body_lines.append(file_path.rsplit('.',1)[0] + ": " + line)
                new_line = ghost_title + " Ghost" + ": " + line
                new_line = new_line.replace("{item}", "{GetOrder()}")
                new_line = new_line.replace("’", "'")
                new_line = new_line.replace("…", "...")
                body_lines.append(new_line)
        body = "\n".join(body_lines)
        if (line.startswith("[Success]")):
            next_action = '<<startStory' + ghost_title + 'Story>>'
        else:
            next_action = '<<setCam "Main Camera">>'

        # Build the Yarn Spinner section
        yarn_section = f"{yarn_title}\n{tags}\n{position}\n---\n{body}\n{next_action}\n===\n"
        yarn_sections.append(yarn_section)

    # Join all sections and return
    return "\n".join(yarn_sections)

# Example usage
input_file = "News Reporter.txt"
output_file = input_file.rsplit('.',1)[0] + "Ghost.yarn"

# Parse the dialogue and write to output file
parsed_output = parse_dialogue(input_file)
with open(output_file, 'w') as file:
    file.write(parsed_output)

print(f"Converted dialogue has been saved to {output_file}")
