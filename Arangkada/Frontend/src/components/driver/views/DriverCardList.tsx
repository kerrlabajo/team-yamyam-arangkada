import { Grid } from "@mui/material";
import DriverCard from "./DriverCard";
import { Driver } from "../../../services/dataTypes";

type DriverCardListProps = {
  drivers: Driver[],
}

const DriverCardList = ({ drivers }: DriverCardListProps) => {
  return (
    <Grid container spacing={2}>
      {drivers.map((driver) => (
        <Grid xs={12} item key={driver.id}>
          <DriverCard
           driver={driver}
          />
        </Grid>
      ))}
    </Grid>
  );
}

export default DriverCardList;