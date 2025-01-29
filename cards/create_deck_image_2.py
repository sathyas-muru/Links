from PIL import Image, ImageDraw, ImageFont

# Dimensions of the playing card (assuming standard poker card size)
CARD_WIDTH = 300
CARD_HEIGHT = 450

# Colors available for the objects (adjusted to darker shades)
COLOR_OPTIONS = {
    'pink': (255, 20, 147),     # Darker pink
    'blue': (30, 144, 255),     # Blue
    'green': (46, 139, 87)      # Darker violet
}

# Object shapes and their abbreviations
OBJECT_SHAPES = {
    'hearts': ('h', '♥'),       # h for heart
    'diamonds': ('d', '♦'),     # d for diamond
    'circle': ('c', '●'),       # c for circle
    'triangle': ('t', '▲')      # t for triangle
}

# Generate all combinations
cards = []
for color, color_rgb in COLOR_OPTIONS.items():
    for shape, (shape_abbr, symbol) in OBJECT_SHAPES.items():
        for count in range(1, 7):  # 1 to 6 objects
            # Create a new blank card image with rounded corners
            card_image = Image.new('RGB', (CARD_WIDTH, CARD_HEIGHT), color='white')
            draw = ImageDraw.Draw(card_image)
            draw.rounded_rectangle([(5, 5), (CARD_WIDTH - 5, CARD_HEIGHT - 5)], radius=20, outline="black", width=5)

            # Determine the positions to place the symbols based on count
            if count == 1:
                positions = [(CARD_WIDTH // 2, CARD_HEIGHT // 2)]
            elif count == 2:
                positions = [(CARD_WIDTH // 2, CARD_HEIGHT // 4), (CARD_WIDTH // 2, 3 * CARD_HEIGHT // 4)]
            elif count == 3:
                positions = [(CARD_WIDTH // 2, CARD_HEIGHT // 6),
                             (CARD_WIDTH // 2, CARD_HEIGHT // 2),
                             (CARD_WIDTH // 2, 5 * CARD_HEIGHT // 6)]
            elif count == 4:
                positions = [(CARD_WIDTH // 3, CARD_HEIGHT // 4),
                             (2 * CARD_WIDTH // 3, CARD_HEIGHT // 4),
                             (CARD_WIDTH // 3, 3 * CARD_HEIGHT // 4),
                             (2 * CARD_WIDTH // 3, 3 * CARD_HEIGHT // 4)]
            elif count == 5:
                positions = [(CARD_WIDTH // 3, CARD_HEIGHT // 6),
                             (2 * CARD_WIDTH // 3, CARD_HEIGHT // 6),
                             (CARD_WIDTH // 2, CARD_HEIGHT // 2),
                             (CARD_WIDTH // 3, 5 * CARD_HEIGHT // 6),
                             (2 * CARD_WIDTH // 3, 5 * CARD_HEIGHT // 6)]
            elif count == 6:
                positions = [(CARD_WIDTH // 4, CARD_HEIGHT // 4),
                             (3 * CARD_WIDTH // 4, CARD_HEIGHT // 4),
                             (CARD_WIDTH // 4, CARD_HEIGHT // 2),
                             (3 * CARD_WIDTH // 4, CARD_HEIGHT // 2),
                             (CARD_WIDTH // 4, 3 * CARD_HEIGHT // 4),
                             (3 * CARD_WIDTH // 4, 3 * CARD_HEIGHT // 4)]

            # Draw the symbols on the card with a slightly smaller font size for triangles
            font_size = 120 if shape == 'triangle' else 150
            font = ImageFont.truetype("arial.ttf", size=font_size)

            for pos in positions:
                draw.text(pos, symbol, fill=color_rgb, font=font, anchor="mm")

            # Save the card image with the specified filename format
            filename = f"{shape_abbr}_{color[0]}_{count}.png"
            card_image.save(filename)
            cards.append(filename)

# Output success message
print(f"Generated {len(cards)} card images.")
