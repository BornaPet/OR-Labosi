# Zagreb Monuments Dataset

The dataset includes a table of monuments and a table of artists. Each monument record contains information such as name, location, type, installation year, material, height, historical significance, popularity, and district. Each artist is linked to one monument and includes attributes such as name, birth year, death year and nationality.

## Metapodaci

- **License**: Creative Commons Attribution-ShareAlike 4.0 International (CC BY-SA 4.0)
- **Author**: Borna Petak
- **Version**: 1.0
- **Language**: Croatian
- **Dataset Description**: The dataset includes a table of monuments and a table of artists. Each monument record contains information such as name, location, type, installation year, material, height, historical significance, popularity, and district. Each artist is linked to one monument and includes attributes such as name, birth year, death year, nationality, and role.
- **Attributes in database:
    - `id`: Unique identifier for each record.
    - `name`: Name of the monument or artist.
    - `location`: Physical location of the monument.
    - `type`: Type of monument (e.g., "statue," "sculpture," "fountain").
    - `year_installed`: Year the monument was installed.
    - `material`: Material the monument is made of.
    - `height`: Height of the monument in meters.
    - `historical_significance`: Brief description of the monument's historical significance.
    - `popularity`: Popularity rating of the monument (1-10).
    - `district`: District of Zagreb where the monument is located.
    - `birth_year`: Artist's birth year.
    - `death_year`: Artist's death year (if applicable).
    - `nationality`: Artist's nationality.

- **Context**: The dataset represents publicly available information useful for tourism, cultural research, and educational purposes.
- **Izvor podataka**: Data was collected based on publicly available sources on monuments in Zagreb.

## File Structure

- `monuments.csv`: CSV format of the monument data.
- `monuments.json`: JSON format of the monument data.
