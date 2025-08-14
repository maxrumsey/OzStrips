import xml.etree.ElementTree as ET
import sys

tree = ET.parse(sys.argv[1])
root = tree.getroot()
groups = root.findall(".//Group")
towers = {}

for group in groups:
    if ("TWR" in group.get("Name")):
        # Look for group fo TWRs
        positions = group.findall(".//Position")
        for pos in positions:
            name = pos.get("Name")
            aerodrome = pos.find(".//ArrivalLists/Airport").get("Name")

            towers[aerodrome] = name

print(towers)

xml = ""

for ad in towers:
    xml += ("<AutoOpen>\n"
        "   <Aerodrome>" + ad + "</Aerodrome>\n"
        "   <Position>" + towers[ad] + "</Position>\n"
        "</AutoOpen>\n"
    )

print(xml)