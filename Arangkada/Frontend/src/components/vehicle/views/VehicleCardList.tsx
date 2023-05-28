import { Grid } from "@mui/material"
import VehicleCard from "./VehicleCard";
import { Vehicle } from "../../../services/dataTypes";

type VehicleCardListProps = {
  vehicles: Vehicle[],
}

const VehicleCardList = ({ vehicles }: VehicleCardListProps) => {
  return ( 
    <Grid container spacing={2}>
      {vehicles.map((vehicles) => (
        <Grid xs={12} md={12} lg={12} item key={vehicles.id}>
          <VehicleCard vehicles={vehicles} />
        </Grid>
      ))}
      
    </Grid>
    
   );
}
 
export default VehicleCardList;