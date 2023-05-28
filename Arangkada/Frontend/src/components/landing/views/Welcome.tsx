import { Stack } from '@mui/material';
import LogoBrown from '../../../images/logobrown.png';
import LogoWhite from '../../../images/logowhite.png';

type MyCompType = {
    line1: string;
    line2: string;
    line3: string;
    isLogin?: boolean;
}

export default function Welcome(props: MyCompType) {
    const { line1, line2, line3, isLogin } = props;

    const logo = isLogin ? LogoWhite : LogoBrown;
    const textColor = isLogin ? '#ffffff' : '#90794C';

    return (
        <Stack spacing={4} width="80%" alignItems="center">
            <img src={logo} alt={"arangkada logo"} style={{width: 150, height: 150}}/>
            <h2 style={{fontSize: '50px', color: textColor, margin: 0}}>{line1}</h2>
            <h1 style={{fontSize: '80px', color: textColor, margin: 0}}>{line2}</h1>
            <i><p style={{ fontSize: "24px", color: textColor, margin: 0}}>{line3}</p></i>
        </Stack>
    )
}
