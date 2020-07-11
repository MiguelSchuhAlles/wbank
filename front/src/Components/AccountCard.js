import React, { Component } from 'react';
import { withStyles  } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';

const useStyles = theme => ({
  root: {
    minWidth: 275,
    width: '25%'
  },
  bullet: {
    display: 'inline-block',
    margin: '0 2px',
    transform: 'scale(0.8)',
  },
  title: {
    fontSize: 14,
  },
  pos: {
    marginBottom: 12,
  },
  horizontal: {
      marginBottom: 50
  }
});
  
class AccountCard extends Component {
  state = {
    name: "",
    balance: 0,
    account: "",
    email: ""
  }

  componentDidMount() {
    if(this.props.token !== ""){
      fetch(`https://localhost:44337/api/users/${this.props.accountId}`,{
        headers: {
           authorization: "Bearer " + this.props.token
         }
      })
      .then(res => res.json())
      .then((data) => {
        console.log(data);

        this.setState({ 
          name: data.name,
          balance: data.account.balance,
          account: data.account.code,
          email: data.email
        });
      })
      .catch(console.log)
    }
  }

  render(){
    const { classes } = this.props;

    return (
      <div className={classes.horizontal}>
        <Card className={classes.root} variant="outlined">
        <CardContent>
            <Typography className={classes.title} color="textSecondary" gutterBottom>
            {this.state.name}
            </Typography>
            <Typography variant="h5" component="h2">
            Balance: $ {this.state.balance.toFixed(2)}
            </Typography>
            <Typography className={classes.pos} color="textSecondary">
            Account {this.state.account}
            </Typography>
            <Typography variant="body2" component="p">
            {this.state.email}
            </Typography>
        </CardContent>
        <CardActions>
            <Button size="small">Help</Button>
        </CardActions>
        </Card>
      </div>
    );
  }
} 


export default withStyles(useStyles)(AccountCard)

