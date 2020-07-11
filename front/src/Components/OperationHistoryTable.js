import React, { Component } from 'react';
import { withStyles } from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TablePagination from '@material-ui/core/TablePagination';
import TableRow from '@material-ui/core/TableRow';   

const columns = [
  { id: 'account', label: 'Account', minWidth: 170 },
  { id: 'amount', label: 'Amount', format: (value) => `$${value.toFixed(2)}`, minWidth: 170 },
  { id: 'balance', label: 'Balance', format: (value) => `$${value.toFixed(2)}`, minWidth: 170 },
  { id: 'date', label: 'Date', minWidth: 170 },
  { id: 'operation', label: 'Operation', minWidth: 170, align: 'right' },
];

const useStyles = theme => ({
  root: {
    width: '70%',
  },
  container: {
    maxHeight: 440,
  },
});

const operationTypes = {
  0: 'Point of Sale',
  1: 'Transfer',
  2: 'Withdrawal',
  3: 'Deposit',
  4: 'Charge',
  5: 'Payment',
  6: 'Interest Income'
}

class OperationHistoryTable extends Component {
  state = {
    page: 0,
    setPage: 0,
    rowsPerPage: 10,
    setRowsPerPage: 10,
    history: []
  }

  componentDidMount() {
    if(this.props.token !== ""){
      fetch(`https://localhost:44337/api/accounts/${this.props.accountId}/history`,{
        headers: {
           authorization: "Bearer " + this.props.token
         }
      })
      .then(res => res.json())
      .then((data) => {
        let processed = data.map(row => {
          return {
            account: row.account.code,
            amount: row.amount,
            balance: row.balance,
            date: row.date,
            operation: operationTypes[row.operationType]            
          };
        });

        this.setState({ history: processed });
      })
      .catch(console.log)
    }
  }

  handleChangePage = (event, newPage) => {
    this.setState({page: newPage})
  };

  handleChangeRowsPerPage = (event) => {
    this.setState({rowsPerPage: +event.target.value, page: 0})
  };

  render(){
    const { classes } = this.props;

    return (
      <Paper className={classes.root}>
        <TableContainer className={classes.container}>
          <Table stickyHeader aria-label="sticky table">
            <TableHead>
              <TableRow>
                {columns.map((column) => (
                  <TableCell
                    key={column.id}
                    align={column.align}
                    style={{ minWidth: column.minWidth }}
                  >
                    {column.label}
                  </TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {this.state.history.slice(this.state.page * this.state.rowsPerPage, this.state.page * this.state.rowsPerPage + this.state.rowsPerPage).map((row) => {
                return (
                  <TableRow hover role="checkbox" tabIndex={-1} key={row.code}>
                    {columns.map((column) => {
                      const value = row[column.id];
                      return (
                        <TableCell key={column.id} align={column.align}>
                          {column.format && typeof value === 'number' ? column.format(value) : value}
                        </TableCell>
                      );
                    })}
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
        <TablePagination
          rowsPerPageOptions={[10, 25, 100]}
          component="div"
          count={this.state.history.length}
          rowsPerPage={this.state.rowsPerPage}
          page={this.state.page}
          onChangePage={this.handleChangePage}
          onChangeRowsPerPage={this.handleChangeRowsPerPage}
        />
      </Paper>);
    }
  }
  

export default withStyles(useStyles)(OperationHistoryTable)