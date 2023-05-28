import { useState } from "react";
import { Button, Grid, InputAdornment, TextField } from "@mui/material";
import DriveEta from "@mui/icons-material/DriveEta";

type SearchFilterFormProps<T> = {
  objectProperties: (keyof T)[];
  data: Partial<T>;
  handleInputChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  handleFilterSubmit: (filters: Partial<T>) => void;
  handleFilterClear: () => void;
};

const SearchFilterForm = <T extends Record<string, any>>({
  objectProperties,
  handleFilterSubmit,
  handleFilterClear,
}: SearchFilterFormProps<T>) => {
  const [searchText, setSearchText] = useState("");

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchText(event.target.value);
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const filters: Partial<T> = {};
    for (const property of objectProperties) {
      filters[property] = searchText as T[keyof T];
    }
    handleFilterSubmit(filters);
  };

  const handleClear = (event: React.MouseEvent) => {
    setSearchText("");
    handleFilterClear();
  };

  return (
    <Grid container spacing={2} onSubmit={handleSubmit} component="form">
      <Grid item xs={12} md={9}>
        <TextField
          onChange={handleInputChange}
          value={searchText}
          name="searchText"
          label="Filter"
          size="small"
          fullWidth
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <DriveEta />
              </InputAdornment>
            ),
          }}
        />
      </Grid>
      <Grid item xs={12} md={3}>
        <Button type="submit" variant="contained" color="secondary" fullWidth>
          Search
        </Button>
      </Grid>
      <Grid item xs={12} md={3}>
        <Button onClick={handleClear} color="error" size="small">
          Clear Filters
        </Button>
      </Grid>
    </Grid>
  );
};

export default SearchFilterForm;
