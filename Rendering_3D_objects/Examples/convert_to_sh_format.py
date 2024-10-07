# Function to convert node coordinates with six decimal places
def convert_nodes(input_lines):
    node_lines = []
    for line in input_lines:
        if line.startswith('n'):
            parts = line.strip().split(',')
            node_id = int(parts[1])
            x, y, z = float(parts[2]), float(parts[3]), float(parts[4])
            # Format the coordinates with six decimal places
            node_lines.append(f"         {node_id}, {x:15.6f}, {y:15.6f}, {z:15.6f}")
    return node_lines

# Function to convert element connectivity
def convert_elements(input_lines):
    element_lines = []
    for line in input_lines:
        if line.startswith('e'):
            parts = line.strip().split(',')
            elem_id = int(parts[1])
            nodes = [int(n) for n in parts[3:]]
            element_lines.append(f"         {elem_id}, " + ", ".join(f"{n:8d}" for n in nodes))
    return element_lines

# Main function to read the input file and write to the output file
def convert_file(input_file, output_file):
    with open(input_file, 'r') as f:
        input_lines = f.readlines()

    # Process nodes and elements
    node_lines = convert_nodes(input_lines)
    element_lines = convert_elements(input_lines)

    # Write to the output file in the desired format
    with open(output_file, 'w') as f:
        # Write nodes
        f.write("*NODE\n")
        for line in node_lines:
            f.write(line + "\n")
        
        # Write elements
        f.write("*ELEMENT,TYPE=S4\n")
        for line in element_lines:
            f.write(line + "\n")

# Example usage
input_file = "klein_bottle_r.txt"
output_file = "klein_bottle_sh.txt"
convert_file(input_file, output_file)