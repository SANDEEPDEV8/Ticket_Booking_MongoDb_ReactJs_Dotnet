import React from "react";
import { CssBaseline, Typography, Container } from "@material-ui/core";
import useStyles from "./FooterStyles";

function Copyright() {
  return (
    <Typography variant="body2" color="textSecondary">
      {/* {"Copyright © "}
      Book Tickets {new Date().getFullYear()}
      {"."} |  */}
      Advanced database systems | Project
    </Typography>
  );
}

export default function StickyFooter() {
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <CssBaseline />
      <footer className={classes.footer}>
        <Container maxWidth="sm">
          <Copyright />
        </Container>
      </footer>
    </div>
  );
}
